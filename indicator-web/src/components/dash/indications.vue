<template>
  <q-page>
    <div class="row" style="min-width: 1800px">
      <div
        class="q-pa-sm"
        style="
          min-width: 290px;
          margin-left: 10px;
          min-height: 100vh;
          background-color: #f2f2f2;
        "
      >
        <div
          class="flex items-center q-pa-sm text-h6"
          style="
            background-color: #dfdfdf;
            min-height: 50px;
            margin-bottom: 5px;
          "
        >
          Potencial
        </div>
        <div
          v-for="indication in indicationsFirstStepArray"
          :key="indication.id"
          class="q-pb-sm"
        >
          <q-card flat bordered class="my-card bg-primary text-white">
            <q-card-section>
              <div class="row items-center no-wrap">
                <div class="col">
                  <div class="text-h6">{{ indication.product.name }}</div>
                  <div class="text-subtitle2">
                    Data de indicação: <br>
                    {{ date(indication.indicationDate) }}
                  </div>
                </div>

                <div class="col-auto">
                  
                </div>
              </div>
            </q-card-section>

            <q-separator />

            <q-card-actions>
              <change-step-btn-component
                :indication="indication"
                v-if="indication"
                @updateStatus="getIndications"
              />
            </q-card-actions>
          </q-card>
        </div>
      </div>
      <div
        class="q-pa-sm"
        style="
          min-width: 290px;
          margin-left: 10px;
          min-height: 100vh;
          background-color: #f2f2f2;
        "
      >
        <div
          class="flex items-center q-pa-sm text-h6"
          style="
            background-color: #dfdfdf;
            min-height: 50px;
            margin-bottom: 5px;
          "
        >
          Primeiro contato
        </div>
        <div
          v-for="indication in indicationsSecondStepArray"
          :key="indication.id"
          class="q-pb-sm"
        >
          <q-card flat bordered class="my-card bg-info text-white">
            <q-card-section>
              <div class="row items-center no-wrap">
                <div class="col">
                  <div class="text-h6">{{ indication.product.name }}</div>
                  <div class="text-subtitle2">
                    Data de indicação: <br>
                    {{ date(indication.indicationDate) }}
                  </div>
                </div>

                <div class="col-auto">
                  
                </div>
              </div>
            </q-card-section>

            <q-separator />

            <q-card-actions>
              <change-step-btn-component
                :indication="indication"
                v-if="indication"
                @updateStatus="getIndications"
              />
              
            </q-card-actions>
          </q-card>
        </div>
      </div>
      <div
        class="q-pa-sm"
        style="
          min-width: 290px;
          margin-left: 10px;
          min-height: 100vh;
          background-color: #f2f2f2;
        "
      >
        <div
          class="flex items-center q-pa-sm text-h6"
          style="
            background-color: #dfdfdf;
            min-height: 50px;
            margin-bottom: 5px;
          "
        >
          Proposta feita
        </div>
        <div
          v-for="indication in indicationsThirdStepArray"
          :key="indication.id"
          class="q-pb-sm"
        >
          <q-card flat bordered class="my-card bg-warning text-white">
            <q-card-section>
              <div class="row items-center no-wrap">
                <div class="col">
                  <div class="text-h6">{{ indication.product.name }}</div>
                  <div class="text-subtitle2">
                    Data de indicação: <br>
                    {{ date(indication.indicationDate) }}
                  </div>
                </div>

                <div class="col-auto">
                  
                </div>
              </div>
            </q-card-section>

            <q-separator />

            <q-card-actions>
              <change-step-btn-component
                :indication="indication"
                v-if="indication"
                @updateStatus="getIndications"
              />
              
            </q-card-actions>
          </q-card>
        </div>
      </div>
      <div
        class="q-pa-sm"
        style="
          min-width: 290px;
          margin-left: 10px;
          min-height: 100vh;
          background-color: #f2f2f2;
        "
      >
        <div
          class="flex items-center q-pa-sm text-h6"
          style="
            background-color: #dfdfdf;
            min-height: 50px;
            margin-bottom: 5px;
          "
        >
          Negociação
        </div>
        <div
          v-for="indication in indicationsFourthtepArray"
          :key="indication.id"
          class="q-pb-sm"
        >
          <q-card flat bordered class="my-card bg-secondary text-white">
            <q-card-section>
              <div class="row items-center no-wrap">
                <div class="col">
                  <div class="text-h6">{{ indication.product.name }}</div>
                  <div class="text-subtitle2">
                    Data de indicação: <br>
                    {{ date(indication.indicationDate) }}
                  </div>
                </div>

                <div class="col-auto">
                  
                </div>
              </div>
            </q-card-section>

            <q-separator />

            <q-card-actions>
              <change-step-btn-component
                :indication="indication"
                v-if="indication"
                @updateStatus="getIndications"
              />
              
            </q-card-actions>
          </q-card>
        </div>
      </div>
      <div
        class="q-pa-sm"
        style="
          min-width: 290px;
          margin-left: 10px;
          min-height: 100vh;
          background-color: #f2f2f2;
        "
      >
        <div
          class="flex items-center q-pa-sm text-h6"
          style="
            background-color: #dfdfdf;
            min-height: 50px;
            margin-bottom: 5px;
          "
        >
          Negócio fechado
        </div>
        <div
          v-for="indication in indicationsFifthStepArray"
          :key="indication.id"
          class="q-pb-sm"
        >
<q-card flat bordered class="my-card bg-positive text-white">
            <q-card-section>
              <div class="row items-center no-wrap">
                <div class="col">
                  <div class="text-h6">{{ indication.product.name }}</div>
                  <div class="text-subtitle2">
                    Data de indicação: <br>
                    {{ date(indication.indicationDate) }}
                  </div>
                  <div class="text-subtitle2" v-if="indication.payments && indication.payments.length > 0">
                    Último pagamento:<br>
                    {{date(indication.payments[indication.payments.length -1].paymentDate).split("-")[0]}}
                  </div>
                </div>

                <div class="col-auto">
                  <q-btn color="white" round flat icon="more_vert">
                    <q-menu cover auto-close>
                      <q-list>
                        <q-item clickable @click="isPaymentOpen = true">
                          <q-item-section>Informar pagamento</q-item-section>
                        </q-item>
                      </q-list>
                    </q-menu>
                  </q-btn>
                </div>
                <payment-dialog
                :indication="indication"
                :isOpen="isPaymentOpen"
                @close="isPaymentOpen = false"/>
              </div>
            </q-card-section>

            <q-separator />

            <q-card-actions>
              <change-step-btn-component
                :indication="indication"
                v-if="indication"
                @updateStatus="getIndications"
              />
              
            </q-card-actions>
          </q-card>
        </div>
      </div>
      <div
        class="q-pa-sm"
        style="
          min-width: 290px;
          margin-left: 10px;
          min-height: 100vh;
          background-color: #f2f2f2;
        "
      >
        <div
          class="flex items-center q-pa-sm text-h6"
          style="
            background-color: #dfdfdf;
            min-height: 50px;
            margin-bottom: 5px;
          "
        >
          Negócio recusado
        </div>
        <div
          v-for="indication in indicationsSixthStepArray"
          :key="indication.id"
          class="q-pb-sm"
        >
<q-card flat bordered class="my-card bg-negative text-white">
            <q-card-section>
              <div class="row items-center no-wrap">
                <div class="col">
                  <div class="text-h6">{{ indication.product.name }}</div>
                  <div class="text-subtitle2">
                    {{ date(indication.indicationDate) }}
                  </div>
                </div>

                <div class="col-auto">
                  <q-btn color="white" round flat icon="more_vert">
                    <q-menu cover auto-close>
                      <q-list>
                        <q-item clickable>
                          <q-item-section>Fazer contato com indicador</q-item-section>
                        </q-item>
                      </q-list>
                    </q-menu>
                  </q-btn>
                </div>
              </div>
            </q-card-section>

            <q-separator />

            <q-card-actions>
              <change-step-btn-component
                :indication="indication"
                v-if="indication"
                @updateStatus="getIndications"
              />
              
            </q-card-actions>
          </q-card>
        </div>
      </div>
    </div>
  </q-page>
</template>

<script>
import ChangeStepBtnComponent from "../commons/change-status-btn";
import PaymentDialog from '../../components/payments/insert-payment'
import UtilService from "../../utils/util.service";

const utilService = new UtilService();
export default {
  name: "DashIndicationsComponent",
  props: [
    "indicationsFirstStepArray",
    "indicationsSecondStepArray",
    "indicationsThirdStepArray",
    "indicationsFourthtepArray",
    "indicationsFifthStepArray",
    "indicationsSixthStepArray",
  ],
  components: {
    ChangeStepBtnComponent,
    PaymentDialog
  },
  data() {
    return {
      isPaymentOpen: false
    };
  },
  methods: {
    date(dateComing) {
      return utilService.convertDate(dateComing);
    },
    getIndications() {
      this.$emit("updateStatus");
    },
  },
};
</script>
