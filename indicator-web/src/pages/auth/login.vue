<template>
  <q-page padding class="row justify-center items-center bg-secondary">
    <div class="col-sm-4 col-xs-12">
      <login-form
      @emitLogin="login"/>
    </div>
  </q-page>
</template>

<script>
import AuthService from '../../services/auth.service'
import NotifyService from '../../utils/notify.service'
import LoginForm from '../../components/auth/login'
const authService = new AuthService()
const notifyService = new NotifyService()
export default {
  name: 'Login',
  components: {
    LoginForm
  },
  data () {
    return {

    }
  },
  methods: {
    login (loginForm) {
      this.$q.loading.show()
      authService.userLogin(loginForm).then(res => {
        this.$q.loading.hide()
        if (res.data) {
          authService.login(res.data)
        }
        this.$router.go('/home')
      }).catch (e => {
        this.$q.loading.hide()
        notifyService.notify(e.response.data.error, 'negative')
      })
    },
    userIsLogged() {
      if (authService.userIsLogged()) {
        this.$router.push('/home')
      }
    }
  },
  created () {
    this.userIsLogged()
  }
}
</script>
