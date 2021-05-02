<template>
  <q-page padding class="row justify-start items-start">
    <div class="col-10">
      <div class="flex items-center" style="height: 50px">
        <div class="q-mr-lg text-h5">Indicações | andamento</div>
        <div v-if="user.userRole == 1" class="flex">
          <div class="q-mr-lg column">
            <span>Data inicial:</span>
            <q-input v-model="filterComp.initialDate" filled type="date" @ />
          </div>
          <div class="column">
            <span>Data final:</span>
            <div class="flex justify-center items-center">
              <q-input
                v-model="filterComp.finishDate"
                filled
                type="date"
                class="q-mr-md"
              />
              <!-- <q-btn round color="primary" icon="search" @click="filterCards" /> -->
            </div>
          </div>
        </div>
      </div>
      <div class="row" style="height: 50px"></div>
      <dash-indications-component
        v-if="
          indicationsFirstStep.length > 0 ||
          indicationsSecondStep.length > 0 ||
          indicationsThirdStep.length > 0 ||
          indicationsFourthtep.length > 0 ||
          indicationsFifthStep.length > 0 ||
          indicationsSixthStep.length > 0
        "
        :indicationsFirstStepArray="indicationsFirstStep"
        :indicationsSecondStepArray="indicationsSecondStep"
        :indicationsThirdStepArray="indicationsThirdStep"
        :indicationsFourthtepArray="indicationsFourthtep"
        :indicationsFifthStepArray="indicationsFifthStep"
        :indicationsSixthStepArray="indicationsSixthStep"
        @updateStatus="getIndications"
      />
    </div>
  </q-page>
</template>
<script>
import DashIndicationsComponent from "../components/dash/indications";
import DashService from "../services/dash.service";
import AuthService from "../services/auth.service";

const dashService = new DashService();
const authService = new AuthService();
export default {
  name: "DashIndications",
  components: {
    DashIndicationsComponent,
  },
  data() {
    return {
      companiesIds: [],
      indicationsArray: [],
      filter: {
        initialDate: "",
        finishDate: "",
      },
      poll: "",
    };
  },
  created() {
    this.mountDateFilter();
    this.createCompanyList();
    this.getWhenCreatePageIndications();
    this.doPoll();
  },
  methods: {
    filterCards() {
      console.log(JSON.stringify(this.filter));
    },
    mountDateFilter() {
      let firstDate = new Date();
      firstDate.setMonth(new Date().getMonth() - 3);
      let monthThreeAgo =
        firstDate.getMonth() + 1 < 10
          ? "0" + (firstDate.getMonth() + 1)
          : firstDate.getMonth() + 1;

      let firstDateFormat =
        firstDate.getFullYear() +
        "-" +
        monthThreeAgo +
        "-" +
        firstDate.getDate();

      let monthCurrent =
        new Date().getMonth() + 1 < 10
          ? "0" + (new Date().getMonth() + 1)
          : new Date().getMonth() + 1;

      let finishDate =
        new Date().getFullYear() +
        "-" +
        monthCurrent +
        "-" +
        new Date().getDate();

      this.filter.initialDate = firstDateFormat.toString();
      this.filter.finishDate = finishDate.toString();

      console.log(JSON.stringify(this.filter));
    },
    doPoll() {
      let vm = this;
      this.poll = setInterval(function () {
        vm.getIndications();
      }, 10000);
    },
    createCompanyList() {
      for (let c of this.user.userCompanies) {
        this.companiesIds.push(c.companyId);
      }
    },
    getIndications() {
      dashService
        .getIndicationsByProduct(this.companiesIds, this.filter)
        .then((res) => {
          this.indicationsArray = res.data;
        })
        .catch((e) => {});
    },
    getWhenCreatePageIndications() {
      this.$q.loading.show();
      dashService
        .getIndicationsByProduct(this.companiesIds, this.filter)
        .then((res) => {
          this.$q.loading.hide();
          this.indicationsArray = res.data;
        })
        .catch((e) => {
          this.$q.loading.hide();
        });
    },
  },
  computed: {
    filterComp() {
      this.getWhenCreatePageIndications();
      return this.filter;
    },
    user() {
      return authService.getUserLogged();
    },
    indicationsFirstStep() {
      return this.indicationsArray.filter((x) => x.status == 0);
    },
    indicationsSecondStep() {
      return this.indicationsArray.filter((x) => x.status == 1);
    },
    indicationsThirdStep() {
      return this.indicationsArray.filter((x) => x.status == 2);
    },
    indicationsFourthtep() {
      return this.indicationsArray.filter((x) => x.status == 3);
    },
    indicationsFifthStep() {
      return this.indicationsArray.filter((x) => x.status == 4);
    },
    indicationsSixthStep() {
      return this.indicationsArray.filter((x) => x.status == 5);
    },
  },
};
</script>
