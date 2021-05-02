<template>
  <q-page padding class="row justify-center items-start">
    <div class="col-10">
      <div style="height: 50px"></div>
      <companies-list-component v-if="companies" :companiesArray="companies" @updateList="getCompaniesToList" :usersArray="companies" :total="total"    />
    </div>
  </q-page>
</template>

<script>
import CompaniesListComponent from "../../components/company/companies-list";
import CompaniesService from "../../services/companies.service";
import NotifyService from "../../utils/notify.service"

const companiesService = new CompaniesService();
const notifyService = new NotifyService();
export default {
  name: "CompaniesList",
  data () {
    return {
      companiesArray: [],
      total: 0
    }
  },
  components: {
    CompaniesListComponent,
  },
  created() {
    this.getCompaniesToList(1, 2);
  },
  methods: {
    getCompaniesToList(page, rowsPerPage) {
      this.$q.loading.show();
      companiesService.getCompanies(page, rowsPerPage).then((res) => {
        this.$q.loading.hide();
        let vm = this
        vm.companiesArray = res.data.companyRegisters
        this.total = res.data.total
      }).catch(e => {
        this.$q.loading.hide();
        this.companiesArray = []
      });
    },
  },
  computed: {
    companies () {
      return this.companiesArray
    }
  }
};
</script>
