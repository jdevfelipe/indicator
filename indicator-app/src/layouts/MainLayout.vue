<template>
  <q-layout view="lHh Lpr lFf">
    <q-header elevated v-if="$router.currentRoute.path != '/validate'">
      <q-toolbar>
        <q-btn
          flat
          dense
          round
          icon="menu"
          aria-label="Menu"
          @click="drawer = !drawer"
          v-if="$router.currentRoute.path != '/auth'"
        />
        <q-toolbar-title>
          {{ this.$router.currentRoute.path.replace('/', '') == 'auth' ? 'indicator' : this.$router.currentRoute.path.replace('/', '') }}
        </q-toolbar-title>

        <!-- <q-btn flat icon="notifications" v-if="$router.currentRoute.path != '/auth'">
          <q-badge floating color="red">2</q-badge>
        </q-btn> -->

        <div v-if="$router.currentRoute.path != '/auth'">
          <q-btn flat color="white" label="Sair" @click="logout()" no-caps />
        </div>

        <div v-if="$router.currentRoute.path == '/auth'">
          <q-btn flat color="white" label="Ajuda" no-caps />
        </div>

      </q-toolbar>
    </q-header>
      <q-drawer
        v-model="drawer"
        show-if-above
        v-if="$router.currentRoute.path != '/auth' && $router.currentRoute.path != '/validate'"
        :width="200"
        :breakpoint="400"
      >
        <q-scroll-area style="height: calc(100% - 150px); margin-top: 150px; border-right: 1px solid #ddd">
          <q-list padding v-for="(menuItem, index) in menu" :key="index">
            <q-item clickable v-ripple :active="menuItem.route === $router.currentRoute.path" @click="$router.push(menuItem.route)">
              <q-item-section avatar>
                <q-icon :name="menuItem.icon" />
              </q-item-section>
              <q-item-section>
                {{menuItem.name}}
              </q-item-section>
            </q-item>
          </q-list>
        </q-scroll-area>

        <q-img class="absolute-top" src="https://cdn.quasar.dev/img/material.png" style="height: 150px" v-if="indicator != null">
          <div class="absolute-bottom bg-transparent">
            <q-avatar color="primary" text-color="white">{{indicator.name.charAt(0).toUpperCase()}}</q-avatar>
            <div class="text-weight-bold">{{indicator.name}}</div>
            <div>{{indicator.email}}</div>
          </div>
        </q-img>
      </q-drawer>
    <q-page-container>
      <router-view />
    </q-page-container>
    <q-page-sticky position="bottom-left" :offset="[18, 18]" v-if="$router.currentRoute.path != '/home' && $router.currentRoute.path != '/auth' && $router.currentRoute.path != '/validate' ">
      <q-btn fab icon="arrow_back" color="primary" @click="hasHistory() ? $router.go(-1) : $router.push('/')"/>
    </q-page-sticky>
  </q-layout>
</template>

<script>
const menu = [
  {
    name: "Início",
    icon: "home",
    route: "/home"
  },
  {
    name: "Produtos",
    icon: "card_giftcard",
    route: "/products/home"
  },
  {
    name: "Indicados",
    icon: "group_add",
    route: "/indications/my-indications"
  },
  {
    name: "Pagamentos",
    icon: "payments",
    route: "/payments/my-payments"
  }
]
import Indicator from '../models/indicator.model'
import AuthService from '../services/auth.service'
const authService = new AuthService()
export default {
  name: 'MainLayout',
  components: {

   },
  data () {
    return {
      drawer: false,
      menu,
      intervalPushNotification: ''
    }
  },
  computed: {
    indicator () {
      return authService.getIndicatorLogged()
    }
  },
  methods: {
    onItemClick () {
      // console.log('Clicked on an Item')
    },
    logout () {
      this.$q.loading.show()
      if(authService.logout() == null) {
        this.$router.push('/auth')
      }
      this.$q.loading.hide()
    },
    hasHistory () {
      return window.history.length > 2 }
    },
    hasPushNotify() {
      let vm = this
      this.intervalPushNotification = setInterval(function(){
          vm.getIndications()
         }, 10000);
    }
}
</script>
