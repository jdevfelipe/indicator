<template>
  <q-page padding>
    <q-stepper
      v-model="step"
      ref="stepper"
      color="primary"
      contracted
      animated
    >
      <q-step
        :name="1"
        :done="done1"
        title="register"
        class="q-pa-none"
      >
        <div class="row">
          <div class="col-12 q-pa-sm">
            <div class="q-gutter-md">
              <q-input ref="name" outlined v-model="registerForm.name" label="Nome" type="text" :rules="[val => !!val || 'campo necessário']" />
            </div>
          </div>
          <div class="col-12 q-pa-sm">
            <div class="q-gutter-md">
              <q-input ref="email" outlined v-model="registerForm.email" label="Email" type="text" :rules="[val => !!val || 'campo necessário']" />
            </div>
          </div>
          <div class="col-12 q-pa-sm">
            <div class="q-gutter-md">
              <q-input ref="cpf" outlined mask="###.###.###-##" v-model="registerForm.CPF" label="CPF" :rules="[val => !!val || 'campo necessário']" />
            </div>
          </div>
          <div class="col-12 q-pa-sm">
            <div class="q-gutter-md">
              <q-input ref="password" outlined v-model="registerForm.password" label="Senha" :rules="[val => !!val || 'campo necessário']"  type="password"/>
            </div>
          </div>
          <q-stepper-navigation>
            <div class="col-12 flex justify-end q-pa-sm">
              <q-btn color="primary" label="Próximo" @click="goToPaymentAccount"/>
            </div>
          </q-stepper-navigation>
        </div>
      </q-step>

      <q-step
        :name="2"
        :done="done2"
        title="payment-account"
      >
        <div class="row">
          <div class="col-12 q-pa-sm">
            <div class="q-gutter-md">
              <q-select outlined v-model="registerForm.paymentAccounts[0].Bank" :options="options" label="Banco" />
            </div>
          </div>
          <div class="col-12 q-pa-sm">
            <div class="row">
              <q-input class="col-7" style="padding-bottom: 0" ref="agency" mask="####" outlined v-model="registerForm.paymentAccounts[0].Agency" label="Agência" type="text" :rules="[val => !!val || 'campo necessário']" />
              <div class="col-2"></div>
              <q-input class="col-3" style="padding-bottom: 0" mask="#" outlined v-model="digitVerif" label="DV" type="text" />
            </div>
          </div>
          <div class="col-12 q-pa-sm">
            <div class="q-gutter-md">
              <q-input ref="account" mask="################" outlined v-model="registerForm.paymentAccounts[0].Account" label="Conta" :rules="[val => !!val || 'campo necessário']" />
            </div>
          </div>
          <q-stepper-navigation>
            <div class="col-12 flex justify-end q-pa-sm">
              <q-btn color="primary" label="Próximo" @click="onIndicatorRegister"/>
              <q-btn v-if="step > 1" flat color="primary" @click="$refs.stepper.previous()" label="Voltar" class="q-ml-sm" />
            </div>
          </q-stepper-navigation>
        </div>
      </q-step>
    </q-stepper>
  </q-page>
</template>

<script>
import AuthService from '../../services/auth.service'
import Indicator from '../../models/indicator.model'
import NotifyService from '../../utils/notify.service'
import Util from '../../utils/util'

const authService = new AuthService()
const notifyService = new NotifyService()
const util = new Util()
export default {
  name: 'Register',
  data() {
    return {
      registerForm: {
        name: "",
        email: "",
        CPF: "",
        password: "",
        paymentAccounts: [{
            Bank: "",
            Agency: "",
            Account: "",
            imageURI: "",
            Status: 0
          }],
        isConfirmed: 1,
        confirmCode: 0
      },
      digitVerif: "",
      indicator: {},
      step: 1,
      done1: false,
      done2: false,
      options: util.banksArr
    }
  },
  methods: {
    goToPaymentAccount () {
      this.$refs.name.validate()
      this.$refs.email.validate()
      this.$refs.cpf.validate()
      this.$refs.password.validate()

      if (this.$refs.name.hasError || this.$refs.email.hasError || this.$refs.cpf.hasError || this.$refs.password.hasError) {
        this.formHasError = true
        return
      }
      this.done1 = true;
      this.step = 2;
    },
    onIndicatorRegister () {
      this.$refs.agency.validate()
      this.$refs.account.validate()

      if (this.$refs.agency.hasError || this.$refs.account.hasError) {
        this.formHasError = true
        return
      }

      if (this.digitVerif) {
        this.registerForm.paymentAccounts[0].Agency = this.registerForm.paymentAccounts[0].Agency + this.digitVerif
      }

      this.registerForm.CPF = this.registerForm.CPF.replace(/\D/g,'');

      this.$q.loading.show()
      this.indicator = new Indicator(this.registerForm)
      authService.registerNewIndicator(this.indicator.data).then(res => {
      this.$q.loading.hide()
      this.$emit('onRegisterSuccess')
      notifyService.notify('Registrado com sucesso', 'positive')
      }).catch(e => {
        console.log(JSON.stringify(e))
        this.$q.loading.hide()
        if (e.response) {
          notifyService.notify(e.response.data.error, 'negative')
        }
      })
    }
  },
}
</script>
<style scoped>
.q-stepper {
  box-shadow: none !important;
}
</style>
