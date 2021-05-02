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
      </div>

      <q-stepper-navigation>
        <q-btn
          @click="emitRegisterStepOne()"
          color="primary"
          label="Continue"
        />
      </q-stepper-navigation>
    </q-step>

    <q-step
      :name="2"
      title="Dados da empresa"
      caption="Obrigatório"
      icon="create_new_folder"
      :done="done2"
    >
      <div class="row justify-center">
        <div class="col-xs-12 q-ma-sm">
          <q-input
            ref="corporateSocialName"
            outlined
            v-model="companyForm.corporateSocialName"
            dense
            label="Razão social"
            style="max-width: 400px !important"
            :rules="[(val) => (val && val.length > 0) || 'Campo obrigatório']"
          />
        </div>
        <div class="col-xs-12 q-ma-sm">
          <q-input
            ref="fantasyName"
            outlined
            v-model="companyForm.fantasyName"
            dense
            label="Nome fantasia"
            style="max-width: 400px !important"
            :rules="[(val) => (val && val.length > 0) || 'Campo obrigatório']"
          />
        </div>
        <div class="col-xs-12 q-ma-sm">
          <q-input
            ref="cnpj"
            outlined
            v-model="companyForm.cnpj"
            dense
            label="CNPJ"
            mask="##.###.###/####-##"
            style="max-width: 400px !important"
            :rules="[(val) => (val && val.length > 0) || 'Campo obrigatório']"
          />
        </div>
        <div class="col-xs-12 q-ma-sm">
          <q-input
            outlined
            v-model="companyForm.cpf"
            dense
            mask="###.###.###-##"
            label="CPF do administrador"
            style="max-width: 400px !important"
          >
            <q-tooltip content-class="bg-secondary" content-style="font-size: 14px" :offset="[10, 10]">
              Você deve ter cadastrado um administrador anteriormente para
              vincula-lo com esta empresa!
            </q-tooltip>
          </q-input>
        </div>
      </div>

      <q-stepper-navigation>
        <q-btn
          @click="emitRegisterStepTwo()"
          color="primary"
          label="Continue"
        />
        <q-btn
          flat
          @click="step = 1"
          color="primary"
          label="Voltar"
          class="q-ml-sm"
        />
      </q-stepper-navigation>
    </q-step>
  </q-stepper>
</template>

<script>
import AuthService from "../../services/auth.service";
import NotifyService from "../../utils/notify.service";

const authService = new AuthService();
const notifyService = new NotifyService();
export default {
  name: "RegisterManagerComponent",
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
        userRole: 1,
      },
      companyForm: {
        corporateSocialName: "",
        fantasyName: "",
        cnpj: "",
        cpf: "",
        imageURI: "",
        Status: 0,
      },
    };
  },
  methods: {
    emitRegisterStepOne() {
      this.$refs.name.validate();
      this.$refs.email.validate();
      this.$refs.cpf.validate();
      this.$refs.password.validate();

      if (!this.formFirstStepIsValid()) {
        return;
      }

      this.registerForm.cpf = this.registerForm.cpf.replace(/\D/g,'');

      authService
        .userRegister(this.registerForm)
        .then((res) => {
          this.$q.loading.hide();
          notifyService.notify("Usuário registrado com sucesso!", "positive");
          this.done1 = true;
          this.step = 2;
        })
        .catch((e) => {
          this.$q.loading.hide();
          notifyService.notify(e.response.data.error, "negative");
        });
    },
    emitRegisterStepTwo() {
      this.$q.loading.show();
      this.$refs.corporateSocialName.validate();
      this.$refs.fantasyName.validate();
      this.$refs.cnpj.validate();

      if (!this.formSecondtStepIsValid()) {
        return;
      }

      this.companyForm.cnpj = this.companyForm.cnpj.replace(/\D/g,'');
      this.companyForm.cpf = this.companyForm.cpf.replace(/\D/g,'');

      authService
        .companyRegister(this.companyForm)
        .then((res) => {
          this.$q.loading.hide();
          notifyService.notify("Empresa registrada com sucesso!", "positive");
          this.done1 = true;
          this.step = 1;
        })
        .catch((e) => {
          this.$q.loading.hide();
          notifyService.notify(e.response.data.error, "negative");
        });
    },
    formFirstStepIsValid() {
      if (this.$refs.name.hasError) {
        this.formHasError = true;
        return false;
      }

      if (this.$refs.email.hasError) {
        this.formHasError = true;
        return false;
      }

      if (this.$refs.cpf.hasError) {
        this.formHasError = true;
        return false;
      }

      if (this.$refs.password.hasError) {
        this.formHasError = true;
        return false;
      }

      return true;
    },
    formSecondtStepIsValid() {
      if (this.$refs.corporateSocialName.hasError) {
        this.formHasError = true;
        return false;
      }

      if (this.$refs.fantasyName.hasError) {
        this.formHasError = true;
        return false;
      }

      if (this.$refs.cnpj.hasError) {
        this.formHasError = true;
        return false;
      }

      return true;
    },
  },
};
</script>
