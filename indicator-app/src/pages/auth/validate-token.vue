<template>
  <q-page class="flex justify-center items-center bg-primary" style="height: 100%;">
    <q-spinner
        color="white"
        size="4em"
      />
  </q-page>
</template>

<script>
import AuthService from '../../services/auth.service'

const authService = new AuthService();
export default {
  name: 'ValidatePage',
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
