<template>
  <q-page padding class="row justify-center items-start">
    <div class="col-10">
      <div class="flex items-center text-h5" style="height: 50px">Empresas | Gerente</div>
      <companies-list-manager-component v-if="companies.length > 0" :companiesArray="companies" @updateList="getCompaniesToList" />
    </div>
  </q-page>
</template>

<script>
import CompaniesListManagerComponent from "../../components/company/companies-list-manager";
import CompaniesService from "../../services/companies.service";
import AuthService from "../../services/auth.service";
import NotifyService from "../../utils/notify.service"

const companiesService = new CompaniesService();
const authService = new AuthService();
const notifyService = new NotifyService();
export default {
  name: "CompaniesListManager",
  data () {
    return {
      companiesArray: [],
      companiesIds: []
    }
  },
  components: {
    CompaniesListManagerComponent,
  },
  created() {
    this.createCompanyList();
    this.getCompaniesToList();
  },
  methods: {
    createCompanyList() {
      for (let c of this.user.userCompanies) {
        this.companiesIds.push(c.companyId);
      }
    },
    getCompaniesToList() {
      this.$q.loading.show();
      companiesService.getCompaniesById(this.companiesIds).then((res) => {
        this.$q.loading.hide();
        let vm = this
        vm.companiesArray = res.data
      }).catch(e => {
        this.$q.loading.hide();
        notifyService.notify(e.response.data.error, 'negative')
      });
    },
  },
  computed: {
    companies () {
      return this.companiesArray
    },
    user () {
      return authService.getUserLogged();
    }
  }
};
</script>
