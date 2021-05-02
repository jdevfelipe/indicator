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
            v-model="registerForm.name"
            dense
            label="Nome"
            style="max-width: 400px !important"
            :rules="[(val) => (val && val.length > 0) || 'Campo obrigatório']"
          />
        </div>
        <div class="col-xs-12 q-ma-sm">
          <q-input
            ref="email"
            outlined
            v-model="registerForm.email"
            dense
            label="E-mail"
            style="max-width: 400px !important"
            :rules="[(val) => (val && val.length > 0) || 'Campo obrigatório']"
          />
        </div>
        <div class="col-xs-12 q-ma-sm">
          <q-input
            ref="cpf"
            outlined
            v-model="registerForm.cpf"
            dense
            label="CPF"
            mask="###.###.###-##"
            style="max-width: 400px !important"
            :rules="[(val) => (val && val.length > 0) || 'Campo obrigatório']"
          />
        </div>
        <div class="col-xs-12 q-ma-sm">
          <q-input
            ref="password"
            outlined
            v-model="registerForm.password"
            dense
            label="Password"
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
            multiple
            v-model="companiesModel"
            dense
            label="Empresa"
            style="max-width: 400px !important"
            :rules="[selectRule]"
          >
            <template v-slot:option="scope">
              <q-item v-bind="scope.itemProps" v-on="scope.itemEvents">
                <q-item-section>
                  <q-item-label v-html="scope.opt.label"></q-item-label>
                </q-item-section>
                <q-item-section side>
                  <q-toggle v-model="companiesModel" :val="scope.opt.value" />
                </q-item-section>
              </q-item>
            </template>
          </q-select>
        </div>
      </div>

      <q-stepper-navigation>
        <q-btn @click="insertNew()" color="primary" label="Continue" />
      </q-stepper-navigation>
    </q-step>
  </q-stepper>
</template>

<script>
import UsersService from "src/services/users.service";
import AuthService from "../../services/auth.service";
import CompaniesService from "../../services/companies.service";
import NotifyService from "../../utils/notify.service";

const authService = new AuthService();
const companiesService = new CompaniesService();
const notifyService = new NotifyService();
const usersService = new UsersService();

export default {
  name: "InsertUserComponent",
  data() {
    return {
      step: 1,
      done1: false,
      done2: false,
      registerForm: {
        name: "",
        email: "",
        cpf: "",
        password: "",
        userEnum: 0,
        userRole: 0,
      },
      companiesModel: [],
      companiesIds: [],
      companiesOptions: [],
    };
  },
  created() {
    this.createCompanyList();
    this.getCompanies();
  },
  methods: {
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

      if (this.$refs.email.hasError) {
        this.formHasError = true;
        return false;
      }

      if (this.$refs.password.hasError) {
        this.formHasError = true;
        return false;
      }

      if (this.$refs.cpf.hasError) {
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
      this.$refs.email.validate();
      this.$refs.password.validate();
      this.$refs.cpf.validate();
      this.$refs.companyId.validate();
      
      if (!this.formIsValid()) {
        notifyService.notify("Preencha os campos marcados!", "negative");
        return;
      }

      this.registerForm.cpf = this.registerForm.cpf.replace(/\D/g,'');

      usersService
        .insertUser(this.registerForm, this.companiesModel)
        .then((res) => {
          notifyService.notify("Usuário inserido com sucesso!", "positive");
          //this.$router.push("/users/list");
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