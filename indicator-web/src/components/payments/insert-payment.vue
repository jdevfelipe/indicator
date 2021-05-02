<template>
  <q-dialog v-model="isOpen" persistent>
    <q-stepper v-model="step" ref="stepper" color="primary" animated>
      <q-step :name="1" title="Informações" icon="settings" :done="step > 1">
        <q-card style="min-width: 350px">
          <q-card-section>
            <div class="text-h6">Informar pagamento!</div>
          </q-card-section>

          <q-card-section class="q-pt-none">
            <q-input
              label="Preço por indicação"
              stack-label
              dense
              v-model="price"
              disable
            />
          </q-card-section>

          <q-card-section class="q-pt-none">
            <q-input
              label="Nome do indicador"
              stack-label
              dense
              v-model="indication.indicator.name"
              disable
            />
          </q-card-section>

          <q-card-section class="q-pt-none">
            <q-input
              label="Email do indicador"
              stack-label
              dense
              v-model="indication.indicator.email"
              disable
            />
          </q-card-section>

          <q-card-section class="q-pt-none">
            <q-input
              label="CPF do indicador"
              stack-label
              dense
              v-model="indication.indicator.cpf"
              disable
            />
          </q-card-section>

          <q-card-section class="q-pt-none">
            <q-input dense v-model="date">
              <template v-slot:append>
                <q-icon name="event" class="cursor-pointer">
                  <q-popup-proxy
                    ref="qDateProxy"
                    transition-show="scale"
                    transition-hide="scale"
                  >
                    <q-date v-model="date" mask="DD/MM/YYYY">
                      <div class="row items-center justify-end">
                        <q-btn
                          v-close-popup
                          label="Close"
                          color="primary"
                          flat
                        />
                      </div>
                    </q-date>
                  </q-popup-proxy>
                </q-icon>
              </template>
            </q-input>
          </q-card-section>
        </q-card>
      </q-step>

      <q-step
        :name="2"
        title="Insira comprovante de pagamento"
        caption="Optional"
        icon="create_new_folder"
        :done="step > 2"
      >
        <q-card style="min-width: 350px">
          <q-card-section class="q-pt-none flex items-center">
            <q-file
              color="purple-12"
              v-model="image"
              stack-label
              label="Clique para inserir comprovante"
            >
              <template v-slot:prepend>
                <q-icon name="attach_file" />
              </template>
              <template v-slot:append>
                <q-icon
                  name="close"
                  @click.stop="image = null"
                  class="cursor-pointer"
                />
              </template>
            </q-file>
          </q-card-section>
        </q-card>
      </q-step>

      <template v-slot:navigation>
        <q-stepper-navigation>
          <q-btn
            @click="step == 1 ? insertPayment() : insertReceipt()"
            color="primary"
            label="Continuar"
          />
          <q-btn
            flat
            color="primary"
            @click="$emit('close')"
            label="Fechar"
            class="q-ml-sm"
          />
        </q-stepper-navigation>
      </template>
    </q-stepper>
  </q-dialog>
</template>

<script>
import PaymentsService from "../../services/payments.service";
import NotifyService from "../../utils/notify.service";

const paymentsService = new PaymentsService();
const notifyService = new NotifyService();
export default {
  name: "PaymentDialog",
  props: ["isOpen", "indication"],
  data() {
    return {
      date: this.nowDate(),
      image: null,
      step: 1,
    };
  },
  computed: {
    price() {
      return "R$" + this.indication.product.indicationPrice;
    },
    paymentForm() {
      return {
        paymentDate: this.date,
        payment: "",
      };
    },
  },
  methods: {
    nowDate() {
      var data = new Date(),
        dia = data.getDate().toString(),
        diaF = dia.length == 1 ? "0" + dia : dia,
        mes = (data.getMonth() + 1).toString(), //+1 pois no getMonth Janeiro começa com zero.
        mesF = mes.length == 1 ? "0" + mes : mes,
        anoF = data.getFullYear();
      return diaF + "/" + mesF + "/" + anoF;
    },
    insertPayment() {
      // const formData = new FormData();
      // formData.append("file", this.image);
      // this.paymentForm.ImageURIOne = formData;
      let day = this.date.split("/")[0];
      let month = this.date.split("/")[1];
      let year = this.date.split("/")[2];

      let date = new Date(year, month - 1, day, 0, 0, 0, 0);
      date.toLocaleString();

      this.paymentForm.paymentDate = date;
      paymentsService
        .insertNewPayment(this.paymentForm, this.indication.id)
        .then((res) => {
          notifyService.notify("Pagamento informado!", "positive");
          this.step = 2;
          this.payment = res.data;
        })
        .catch((e) => {
          notifyService.notify(e.response.data.error, "negative");
        });
    },
    insertReceipt() {
      const formData = new FormData();
      formData.append("file", this.image);
      paymentsService
        .insertNewReceipt(formData, this.payment.id)
        .then((res) => {
          notifyService.notify(
            "Comprovante cadastrado com sucesso!",
            "positive"
          );
          this.image = null;
          this.step = 1;
          this.$emit("close");
        })
        .catch((e) => {
          notifyService.notify(e.response.data.error, "negative");
        });
    },
  },
};
</script>
