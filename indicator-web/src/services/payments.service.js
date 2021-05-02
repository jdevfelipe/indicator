import { http } from "src/boot/axios"

export default class PaymentsService {

    insertNewPayment = async (payment, indicationId) => {
        let config = JSON.parse(localStorage.getItem('user'))
        const response = await http.post('payment/add-new-payment/', JSON.stringify(payment), { headers: { 'Authorization': `Bearer ${config.token}`, 'indication': indicationId}});
        return response
    }

    insertNewReceipt = async (file, paymentId) => {
        let config = JSON.parse(localStorage.getItem('user'))
        const response = await http.post('payment/add-new-receipt/', file, { headers: { 'Authorization': `Bearer ${config.token}`, 'payment': paymentId}});
        return response
    }

    getPaymentsByCompanies = async (payload, page, rowsPerPage) => {
        let config = JSON.parse(localStorage.getItem('user'))
        const response = await http.get('payment/get-payments-by-company/', { headers: { 'Authorization': `Bearer ${config.token}`, 'companiesIds': payload, 'page': page, 'limit': rowsPerPage}});
        return response
    }
}