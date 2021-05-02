<template>
  <q-page padding class="overflow-hidden">
    <div>
      <div class="row">
        <q-tabs
          v-model="tab"
          dense
          class="text-grey"
          active-color="primary"
          indicator-color="primary"
          align="justify"
          narrow-indicator
        >
          <q-tab name="login" label="Sou indicador" />
          <q-tab name="register" label="Criar conta!" />
        </q-tabs>
        <q-separator />
      </div>
    </div>
    <q-tab-panels 
    v-model="tab"
    animated
    swipeable
    transition-prev="jump-right"
    transition-next="jump-left">
      <q-tab-panel name="login" class="q-pa-none">
        <login></login>
      </q-tab-panel>

      <q-tab-panel name="register" class="q-pa-none">
        <register
        @onRegisterSuccess="onRegisterSuccess"></register>
      </q-tab-panel>
    </q-tab-panels>
  </q-page>
</template>

<script>
import Login from '../../components/auth/Login'
import Register from '../../components/auth/Register'
export default {
  name: 'AuthComponent',
  components: {
    Login,
    Register
  },
  data() {
    return {
      tab: 'login'
    }
  },
  methods: {
    onRegisterSuccess () {
      this.tab = 'login'
    }
  },
}
</script>
