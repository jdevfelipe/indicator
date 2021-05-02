<template>
  <q-card flat bordered class="my-card my-shadow">
    <q-card-section class="flex justify-between">
      <div class="text-h6">Login</div>
    </q-card-section>
    <q-card-section class="q-pt-none">
      <q-input outlined v-model="loginForm.cpf" label="CPF" mask="###.###.###-##" />
    </q-card-section>
    <q-card-section>
      <q-input outlined v-model="loginForm.password" label="Senha" type="password" @keydown.enter.prevent="login()"/>
    </q-card-section>
    <q-card-section class="flex justify-between">
      <q-btn flat color="primary" label="Esqueci minha senha" @click="forgotPassShow = true"/>
      <q-btn color="primary" label="Entrar" @click="login()" />
    </q-card-section>
    <forgot-pass-dialog @close="forgotPassShow = false" :show="forgotPassShow"/>
  </q-card>
</template>

<script>
import forgotPassDialog from './forgotPassDialog'
export default {
  name: "LoginComponent",
  components: {
    forgotPassDialog
  },
  data() {
    return {
      loginForm: {
        cpf: "",
        password: "",
      },
      forgotPassShow: false
    };
  },
  methods: {
    login() {
      this.loginForm.cpf = this.loginForm.cpf.replace(/\D/g,'');
      this.$emit("emitLogin", this.loginForm);
    },
  },
};
</script>
