<template>
  <q-page padding class="flex justify-center items-center bg-secondary">
    <q-spinner
        color="white"
        size="3em"
      />
  </q-page>
</template>

<script>
import AuthService from '../../services/auth.service'

const authService = new AuthService()
export default {
  name: 'validate',
  created () {
    if (authService.userIsLogged()) {
      this.$router.push('/home')
    }
  },
  mounted() {
    this.isValid()
  },
  methods: {
    isValid(){
      authService.verifyIfTokenIsValid().then(res => {
        if (res.data == true) {
          this.$router.push('/home')
        } else {
          authService.logout()
          this.$router.push('/auth')
        }
      }).catch(e => {

      })
    }
  },
}
</script>
