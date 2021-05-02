using Azure.Storage.Blobs;
using indicator_api.DTOs;
using indicator_api.Entities;
using indicator_api.Enums;
using indicator_api.Exceptions;
using indicator_api.Repositories;
using indicator_api.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.Services
{
    public class PaymentService : PaymentUtil
    {
        private readonly PaymentRepository _repository;
        private readonly IndicationService _indicationService;

        public PaymentService()
        {
        }

        public PaymentService(PaymentRepository repository, IndicationService indicationService)
        {
            _repository = repository;
            _indicationService = indicationService;
        }

        public Payment AddPayment(int indication, Payment payment)
        {
            PaymentUtil Util = new PaymentService();

            if (indication == 0)
            {
                throw new BadRequestException("Indicação é null");
            }

            payment.IndicationId = indication;

            Indication indicationCanPay = _indicationService.GetById(indication);

            if (Util.ObjectIsNull(indicationCanPay))
            {
                throw new NotFoundException("Nenhum indicação encontrada");
            }
            else if (!indicationCanPay.Status.Equals(IndicationStatus.SUCCESS))
            {
                throw new BadRequestException("Esta indicação não tem status pra ser paga!");
            }

            //string imageURI = null;

            //if (file != null)
            //{
            //    using (var stream = file.OpenReadStream())
            //    {
            //        imageURI = await UploadToBlob(file.FileName, null, stream);
            //    }
            //}

            //if (imageURI != null)
            //{
            //    payment.ImageURIOne = imageURI;
            //}

            try
            {
                _repository.Create(payment);

            }
            catch (Exception e)
            {
                throw new Exception($"Algo deu errado {e}");
            }

            return payment;

        }

        public PaymentPaginated ListPayments(int indicatorId, string term, int page, int limit)
        {
            if (indicatorId.Equals(null) || indicatorId < 1)
            {
                throw new BadRequestException("user nulla");
            }


            List<Payment> payments = new List<Payment>();
            PaymentPaginated paymentPaginated = new PaymentPaginated();
            try
            {

                if (term.Equals("CREATED"))
                {
                    payments = _repository.ListPaymentsByUser(indicatorId, page, limit);
                }
                else
                {
                    payments = _repository.search(indicatorId, term, page, limit);
                }

                int total = _repository.countPayments();
                paymentPaginated.payments = payments;
                paymentPaginated.total = total;

            }
            catch (Exception e)
            {
                throw new Exception($"Algo deu errado: {e}");
            }

            return paymentPaginated;
        }

        public async Task<Payment> InsertReceiptAsync(IFormFile file, int paymentId) 
        {
            PaymentUtil Util = new PaymentService();

            Payment payment = _repository.GetById(paymentId);

            if (Util.ObjectIsNull(payment)) 
            {
                throw new NotFoundException("Não existe pagamento com este Id");
            }

            using (var stream = file.OpenReadStream())
            {
                payment.ImageURIOne = await UploadToBlob(file.FileName, null, stream);
            }

            if (payment.ImageURIOne != null)
            {
                try
                {
                    _repository.Update(payment);
                    return payment;
                }
                catch (Exception e)
                {
                    throw new Exception($"Algo deu errado {e}");
                }
            }
            else 
            {
                throw new BadRequestException("Não foi possível salvar o comprovante");
            }
        }

        private async Task<string> UploadToBlob(string filename, byte[] imageBuffer = null, Stream stream = null)
        {
            CloudStorageAccount storageAccount = null;
            CloudBlobContainer cloudBlobContainer = null;
            string storageConnectionString = Settings.Settings.StorageConnection;

            // Check whether the connection string can be parsed.
            if (CloudStorageAccount.TryParse(storageConnectionString, out storageAccount))
            {
                try
                {
                    // Create the CloudBlobClient that represents the Blob storage endpoint for the storage account.
                    CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

                    // Create a container called 'uploadblob' and append a GUID value to it to make the name unique. 
                    cloudBlobContainer = cloudBlobClient.GetContainerReference("indicator-api");
                    await cloudBlobContainer.CreateIfNotExistsAsync();

                    // Set the permissions so the blobs are public. 
                    BlobContainerPermissions permissions = new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    };
                    await cloudBlobContainer.SetPermissionsAsync(permissions);

                    // Get a reference to the blob address, then upload the file to the blob.
                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(Guid.NewGuid().ToString() + filename);

                    if (imageBuffer != null)
                    {
                        // OPTION A: use imageBuffer (converted from memory stream)
                        await cloudBlockBlob.UploadFromByteArrayAsync(imageBuffer, 0, imageBuffer.Length);
                    }
                    else if (stream != null)
                    {
                        // OPTION B: pass in memory stream directly
                        await cloudBlockBlob.UploadFromStreamAsync(stream);
                    }
                    else
                    {
                        return (null);
                    }

                    string imageURI = cloudBlockBlob.SnapshotQualifiedStorageUri.PrimaryUri.ToString();

                    return (imageURI);
                }
                catch (StorageException ex)
                {
                    return (null);
                }
                finally
                {
                    // OPTIONAL: Clean up resources, e.g. blob container
                    //if (cloudBlobContainer != null)
                    //{
                    //    await cloudBlobContainer.DeleteIfExistsAsync();
                    //}
                }
            }
            else
            {
                return (null);
            }
        }

        public PaymentPaginated GetPaymentsByCompanies(List<int> companiesIds, int page, int limit) 
        {
            if (companiesIds.Count() < 1) 
            {
                throw new BadRequestException("Algo deu errado com esta requisição, atualize a página!");
            }

            List<Payment> payments = new List<Payment>();

            try 
            {
                payments = _repository.GetPaymentsByCompanies(companiesIds, page, limit);

            } catch (Exception e) 
            {
                throw new Exception($"Algo deu errado {e}");
            }

            if (payments.Count() < 1)
            {
                throw new NotFoundException("Nenhum pagamento encontrado");
            }

            PaymentPaginated paymentPaginated = new PaymentPaginated();

            paymentPaginated.payments = payments;
            paymentPaginated.total = _repository.CountPaymentsByCompanies(companiesIds);

            return paymentPaginated;


        }

        public Payment ConfirmReceive(int paymentId) 
        {
            if (paymentId == 0) 
            {
                throw new BadRequestException("Algo deu errado, entre em contato com o suporte!");
            }

            Payment payment = new Payment();

            try 
            {
                payment = _repository.GetById(paymentId);
            } catch (Exception e) 
            {
                throw new Exception($"Algo deu errado {e}");
            }

            if (payment == null) 
            {
                throw new NotFoundException("Nenhum pagamento encontrado :/");
            }

            if (payment.Status.Equals(PaymentStatus.CONFIRMED))
            {
                throw new BadRequestException("Pagamento já foi confirmado");
            }

            payment.Status = PaymentStatus.CONFIRMED;

            try 
            {
                _repository.Update(payment);
            } catch (Exception e) 
            {
                throw new Exception($"Algo deu errado {e}");
            }

            return payment;
        }

        public Payment ShowPay(int paymentId)
        {
            if (paymentId == 0)
            {
                throw new BadRequestException("Algo deu errado, entre em contato com o suporte!");
            }

            Payment payment = new Payment();

            try
            {
                payment = _repository.GetById(paymentId);
            }
            catch (Exception e)
            {
                throw new Exception($"Algo deu errado {e}");
            }

            if (payment == null)
            {
                throw new NotFoundException("Nenhum pagamento encontrado :/");
            }

            return payment;
        }
    }
}
