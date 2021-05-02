<template>
  <q-page padding class="row justify-center items-start">
    <div class="col-10">
      <div style="height: 50px"></div>
      <payments-list-component v-if="payments && totalComp" :paymentsArray="payments" :total="totalComp" @updateList="getPaymentsToList"/>
    </div>
  </q-page>
</template>

<script>
import PaymentsListComponent from "../../components/payments/payment-list";
import AuthService from "../../services/auth.service";
import PaymentsService from "../../services/payments.service";
import NotifyService from "../../utils/notify.service"

const paymentsService = new PaymentsService();
const authService = new AuthService();
const notifyService = new NotifyService();
export default {
  name: "PaymentsList",
  data () {
    return {
      paymentsArray: [],
      companiesIds: [],
      total: 0
    }
  },
  components: {
    PaymentsListComponent,
  },
  created() {
    this.getPaymentsToList(1, 2);
    this.createCompanyList();
  },
  methods: {
    createCompanyList() {
      for (let c of this.user.userCompanies) {
        this.companiesIds.push(c.companyId);
      }
    },
    getPaymentsToList(page, rowsPerPage) {
      this.$q.loading.show();
      paymentsService.getPaymentsByCompanies(this.companiesIds, page, rowsPerPage).then((res) => {
        this.$q.loading.hide();
        let vm = this
        vm.paymentsArray = res.data.payments
        vm.total = res.data.total
      }).catch(e => {
        this.$q.loading.hide();
        this.paymentsArray = []
      });
    },
  },
  computed: {
    payments () {
      return this.paymentsArray
    },
    totalComp () {
      return this.total
    },
    user () {
      return authService.getUserLogged();
    }
  }
};
</script>
