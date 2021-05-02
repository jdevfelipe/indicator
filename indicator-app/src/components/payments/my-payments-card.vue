<template>
  <div>
    <q-card class="my-card">
      <q-card-section :class="[bgCard, 'text-white', 'flex',  'justify-between']">
        <div class="flex column">
          <div class="text-h6">{{item.indication.product.name}}</div>
          <div class="text-subtitle2">Status: {{status}}</div>
          <div class="text-subtitle2">Pago: {{date}}</div>
        </div>
        <q-icon :name="icon" style="font-size: 2em;" />
      </q-card-section>

      <q-separator />

      <q-card-actions>
        <q-btn no-caps flat color="primary" @click="confirmReceive">Confirmar recebimento</q-btn>
        <q-btn no-caps flat color="primary" @click="showPayment" v-if="item.imageURIOne">DETALHES</q-btn>
      </q-card-actions>
    </q-card>
    <payment-dialog :show="show" :img="item.imageURIOne" @close="show = false"/>
  </div>
</template>

<script>
import Util from '../../utils/util'
import NotifyService from '../../utils/notify.service'
import PaymentService from '../../services/payment.service'
import PaymentDialog from '../commons/payment-dialog.vue'

const util = new Util()
const paymentService = new PaymentService()
const notifyService = new NotifyService()
export default {
  name: 'myPaymentsCard',
  components: {
    PaymentDialog
  },
  props: ['item'],
  data () {
    return {
      img: "",
      show: false
    }
  },
  methods: {
    confirmReceive () {
      this.$q.loading.show()
      paymentService.confirmReceive(this.item.id).then(res => {
        this.$q.loading.hide()
        notifyService.notify('Recebimento confirmado com sucesso', 'positive');
        this.$emit('getPayments')
      }).catch(e => {
        this.$q.loading.hide()
        notifyService.notify(e.response.data.error, 'negative')
      })
    },
    showPayment () {
      this.$q.loading.show()
      paymentService.showPayment(this.item.id).then(res => {
        this.$q.loading.hide()
        this.img = res.data.imageURIOne
        this.show = true
      }).catch(e => {
        this.$q.loading.hide()
        notifyService.notify(e.response.data.error, 'negative')
      })
    }
  },
  computed : {
    bgCard () {
      switch (this.item.status) {
        case 0: return 'bg-primary'
          break;
        case 1: return 'bg-secondary'
          break;
        case 2: return 'bg-positive'
          break;
        case 3: return 'bg-warning'
          break;
      }
    },
    status () {
      switch (this.item.status) {
        case 0: return 'AGUARDANDO CONFIRMAÇÃO'
          break;
        case 1: return 'IDENTIFICADO'
          break;
        case 2: return 'IDENTIFICADO E NOTIFICADO'
          break;
        case 3: return 'NÃO IDENTIFICADO'
          break;
      }
    },
    icon () {
      switch (this.item.status) {
        case 0: return 'hourglass_empty'
          break;
        case 1: return 'info'
          break;
        case 2: return 'check_circle_outline'
          break;
        case 3: return 'help_outline'
          break;
      }
    },
    date () {
      return util.formatDate(this.item.paymentDate)
    }
  }
}
</script>
