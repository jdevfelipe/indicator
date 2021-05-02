<template>
  <q-stepper v-model="step" header-nav ref="stepper" color="primary" animated>
    <q-step
      :name="1"
      title="Dados do usuário"
      caption="Obrigatório"
      icon="settings"
      :done="done1"
    >
      <div class="row justify-center">
        <div class="col-xs-12 q-ma-sm">
          <q-input
            ref="name"
            outlined
            v-model="productForm.name"
            dense
            label="Nome"
            style="max-width: 400px !important"
            :rules="[(val) => (val && val.length > 0) || 'Campo obrigatório']"
          />
        </div>
        <div class="col-xs-12 q-ma-sm">
          <q-input
            @input="formatModel"
            type="tel"
            ref="indicationPrice"
            outlined
            reverse-fill-mask
            v-model="productForm.indicationPrice"
            dense
            label="Preço de indicação"
            style="max-width: 400px !important"
            :rules="[(val) => (val && val.length > 0) || 'Campo obrigatório']"
          />
        </div>
        <div class="col-xs-12 q-ma-sm">
          <q-select
            ref="companyId"
            map-options
            emit-value
            :options="companiesOptions"
            outlined
            v-model="productForm.companyId"
            dense
            label="Empresa"
            style="max-width: 400px !important"
            :rules="[selectRule]"
          />
        </div>
      </div>

      <q-stepper-navigation>
        <q-btn @click="insertNew()" color="primary" label="Continue" />
      </q-stepper-navigation>
    </q-step>
  </q-stepper>
</template>

<script>
import ProductsService from "../../services/products.service";
import AuthService from "../../services/auth.service";
import CompaniesService from "../../services/companies.service";
import NotifyService from "../../utils/notify.service";

const productsService = new ProductsService();
const authService = new AuthService();
const companiesService = new CompaniesService();
const notifyService = new NotifyService();

export default {
  name: "InsertProductComponent",
  data() {
    return {
      step: 1,
      done1: false,
      done2: false,
      productForm: {
        name: "",
        indicationPrice: "",
        companyId: null,
        status: 0,
      },
      companiesIds: [],
      companiesOptions: [],
    };
  },
  created() {
    this.createCompanyList();
    this.getCompanies();
  },
  methods: {
    formatModel(val) {
      this.productForm.indicationPrice = val.toLocaleString('pt-br',{style: 'currency', currency: 'BRL'})
    },
    createCompanyList() {
      for (let c of this.user.userCompanies) {
        this.companiesIds.push(c.companyId);
      }
    },
    getCompanies() {
      companiesService
        .getCompaniesById(this.companiesIds)
        .then((res) => {
          for (let c of res.data) {
            let company = {
              value: 0,
              label: "",
            };
            company.value = c.id;
            company.label = c.fantasyName;
            this.companiesOptions.push(company);
          }
        })
        .catch((e) => {
          notifyService.notify(e.response.data.error, "negative");
        });
    },
    selectRule(val) {
      if (!val) {
        return "Campo obrigatório!";
      }
    },
    formIsValid() {
      if (this.$refs.name.hasError) {
        this.formHasError = true;
        return false;
      }

      if (this.$refs.indicationPrice.hasError) {
        this.formHasError = true;
        return false;
      }

      if (this.$refs.companyId.hasError) {
        this.formHasError = true;
        return false;
      }

      return true;
    },
    insertNew() {
      this.$refs.name.validate();
      this.$refs.indicationPrice.validate();
      this.$refs.companyId.validate();

      if (!this.formIsValid()) {
        notifyService.notify("Preencha os campos marcados!", "negative");
        return;
      }

      let formattedPrice = this.productForm.indicationPrice.replace(',', '.');

      this.productForm.indicationPrice = formattedPrice;

      productsService
        .insertProduct(this.productForm)
        .then((res) => {
          notifyService.notify("Produto inserido com sucesso!", "positive");
          this.$router.push("/products/list");
        })
        .catch((e) => {
          notifyService.notify(e.response.data.error, "negative");
        });
    },
  },
  computed: {
    user() {
      return authService.getUserLogged();
    },
  },
};
</script>
