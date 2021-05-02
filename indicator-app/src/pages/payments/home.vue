<template>
  <q-page class="flex column justify-start items-center q-pl-md q-pr-md q-pt-lg" padding>
    <div style="width: 100%;" class="q-mb-lg">
      <searchBar ref="searchBar" @getProductsBySearch="getPayBySearch"/>
    </div>
    <div style="display: contents" v-if="payments.length > 0">
      <div style="width: 100%;" class="q-mb-lg" v-for="payment in payArr">
        <my-payments-card :item="payment" @getPayments="getPayments" />
      </div>
    </div>
    <q-pagination
          v-model="current"
          color="primary"
          :max="maxPages"
          :max-pages="3"
          :ellipses="false"
          :boundary-numbers="false"
          @click="$refs.searchBar.text ? getPayBySearch($refs.searchBar.text) : getPayments('CREATED' , current, 3)"
        >
    </q-pagination>
  </q-page>
</template>

<script>
import MyPaymentsCard from '../../components/payments/my-payments-card'
import PaymentService from '../../services/payment.service'
import searchBar from '../../components/commons/search-bar'

const paymentService = new PaymentService()
export default {
  name: 'MyPayments',
  data () {
    return {
      payments: [],
      current: 1,
      maxPages: 0
    }
  },
  components: {
    MyPaymentsCard,
    searchBar
  },
  methods: {
    getPayments(text) {
      paymentService.getPayments(text, this.current, 3).then(res => {
        this.payments = res.data.payments
        this.maxPages = Math.ceil(res.data.total /3)
      })
    },
    getPayBySearch(text) {
      this.$q.loading.show()
      this.current = 1
      paymentService.getPayments(text, this.current, 3).then(res => {
        debugger
        this.payments = res.data.payments
        this.maxPages = Math.ceil(res.data.total /3)
        this.$q.loading.hide()
      }).catch(e => {
        this.payments = []
        this.$q.loading.hide()
      })

    },
  },
  created () {
    this.getPayments('CREATED', 1, 3)
  },
  computed: {
    payArr () {
      return this.payments
    }
  }
}
</script>
