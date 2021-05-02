<template>
  <q-page padding>
    <div class="row" style="height: 100px;">
      <div class="col-12 flex justify-start items-center q-pa-sm">
        <span class="text-h5 text-primary">
          login
        </span>
      </div>
    </div>
    <div class="row">
          <div class="col-12 q-pa-sm">
            <div class="q-gutter-md">
              <q-input outlined v-model="indicatorForm.cpf" label="CPF" mask="###.###.###-##" />
            </div>
          </div>
          <div class="col-12 q-mb-md q-pa-sm">
            <div class="q-gutter-md">
              <q-input outlined v-model="indicatorForm.password" label="Senha" type="password" />
            </div>
          </div>
          <div class="col-12 flex justify-between q-pa-sm">
            <q-btn size="sm" outline  class="q-pa-none" color="primary" label="Esqueci minha senha" @click="forgotPassShow = true" />
            <q-btn color="primary" label="Entrar" @click="onIndicatorLogin"/>
          </div>      
    </div>
    <dialog-confirm-code
     :isConfirmed='isConfirmed'
     :indicatorForm='indicatorForm'
     @closeDialogConfirm="isConfirmed = false"/>
     <forgot-pass-dialog @close="forgotPassShow = false" :show="forgotPassShow" />
  </q-page>
</template>

<script>
import AuthService from '../../services/auth.service'
import NotifyService from '../../utils/notify.service'
import Indicator from '../../models/indicator.model'
import DialogConfirmCode from '../../components/auth/dialog-confirm-code'
import ForgotPassDialog from '../auth/forgotPass'

const authService = new AuthService()
const notifyService = new NotifyService()
export default {
  name: 'Login',
  components: {
      DialogConfirmCode,
      ForgotPassDialog
  },
  data () {
    return {
      text: '',
      indicatorForm: {
        cpf: '',
        password: '',
      },
      indicator: {},
      isConfirmed: false,
      forgotPassShow: false
    }
  },
  methods: {
    onIndicatorLogin () {
      this.$q.loading.show()
      this.indicatorForm.cpf = this.indicatorForm.cpf.replace(/\D/g,'');
      authService.indicatorLogin(this.indicatorForm).then(res => {
        this.indicator = new Indicator(res.data)
        authService.login(this.indicator.data)
        this.$router.go('/home')
        this.$q.loading.hide()
      }).catch(e => {
        if (e.response.data.error == 'NOT_CONFIRMED') 
        {
          this.$q.loading.hide()
          this.isConfirmed = true
          return
        }
        this.$q.loading.hide()
        notifyService.notify(e.response.data.error, 'negative')
      })
    },
    indicatorIsLogged () {
      if (authService.indicatorIsLogged()) {
        this.$router.push('/home')
      }
    }
  },
  created () {
    this.indicatorIsLogged()
  }
}
</script>
