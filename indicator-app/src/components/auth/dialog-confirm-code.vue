<template>
<q-page>
  <q-dialog v-model="isConfirmed" persistent>
      <q-card style="min-width: 300px">
        <q-card-section>
          <div class="text-h6">Confirme sua conta!</div>
          <div class="text-subtitle2">Digite o código que recebeu em seu email ;)</div>
        </q-card-section>

        <q-card-section class="q-pt-none">
          <q-input type="number" dense v-model="code" autofocus />
        </q-card-section>

        <q-card-actions align="right" class="text-primary">
          <q-btn outline label="Reenviar"  @click="reSendCode()"/>
          <q-btn flat label="Cancelar" @click="($emit('closeDialogConfirm'))" />
          <q-btn flat label="Confirmar" @click="verifyCode()" />
        </q-card-actions>
      </q-card>
    </q-dialog>
    <dialog-code-response
    :codeIsOk='codeIsOk'
    :title='title'
    :message='message'
    :type='type'
    @closeDialogCodeResponse="closeDialogCodeResponse"
    @proceed="proceed"/>
</q-page>
</template>

<script>
import AuthService from '../../services/auth.service'
import NotifyService from '../../utils/notify.service'
import Indicator from '../../models/indicator.model'
import DialogCodeResponse from '../../components/auth/dialog-code-response'

const authService = new AuthService()
const notifyService = new NotifyService()
export default {
  name: 'dialogConfirmCode',
  props: ['isConfirmed', 'indicatorForm'],
  components: {
    DialogCodeResponse
  },
  data () {
    return {
      code: '',
      indicator: {},
      codeIsOk: false,
      title: '',
      message: '',
      type: ''
    }
  },
  methods: {
    verifyCode(){
      this.$q.loading.show()
      this.indicatorForm.cpf = this.indicatorForm.cpf.replace(/\D/g,'');
      authService.verifyIfCodeIsOK(this.indicatorForm, this.code).then(res => {
        this.indicator = new Indicator(res.data)
        authService.login(this.indicator.data)
        this.$q.loading.hide()
        this.title = 'Tudo certo!'
        this.message = 'Agora você pode continuar!'
        this.type = 'OK'
        this.codeIsOk = true
      }).catch(e => {
        this.$q.loading.hide()
        this.title = 'Ops :c'
        this.message = e.response.data.error
        this.type = 'NOT'
        this.codeIsOk = true
      })
    },
    reSendCode(){
      this.$q.loading.show()
      authService.generateNewCode(this.indicatorForm).then(res => {
        this.$q.loading.hide()
        notifyService.notify('Ok, confira seu email!', 'positive')
      }).catch(e => {
        notifyService.notify(e.response.data.error, 'negative')
        this.$q.loading.hide()
      })
    },
    closeDialogCodeResponse(){
      this.codeIsOk = false      
    },
    proceed () {
      this.$router.go('/home')
    }
  },
}
</script>
