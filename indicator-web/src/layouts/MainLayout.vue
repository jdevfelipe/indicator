<template>
  <q-layout view="lHh Lpr lFf">
    <q-header
      elevated
      class="bg-secondary"
      v-if="
        $router.currentRoute.path != '/validate' &&
        $router.currentRoute.path != '/auth/login'
      "
    >
      <q-toolbar>
        <q-btn
          flat
          dense
          round
          icon="menu"
          aria-label="Menu"
          @click="drawer = !drawer"
        />

        <q-toolbar-title> Indicator </q-toolbar-title>

        <q-btn flat label="Sair" @click="logout()" no-caps />
      </q-toolbar>
    </q-header>

    <q-drawer
      v-model="drawer"
      show-if-above
      :width="200"
      :breakpoint="400"
      v-if="
        $router.currentRoute.path != '/validate' &&
        $router.currentRoute.path != '/auth/login'
      "
    >
      <q-scroll-area
        style="
          height: calc(100% - 150px);
          margin-top: 150px;
          border-right: 1px solid #ddd;
        "
      >
        <div v-for="(item, index) in linksData" :key="index">
          <q-list padding v-if="item.canShow">
            <q-item clickable v-ripple  :to="item.link">
              <q-item-section avatar>
                <q-icon :name="item.icon" />
              </q-item-section>
              <q-item-section>{{ item.title }}</q-item-section>
            </q-item>
          </q-list>
        </div>
      </q-scroll-area>

      <q-img
        class="absolute-top"
        src="https://cdn.quasar.dev/img/material.png"
        style="height: 150px"
      >
        <div class="absolute-bottom bg-transparent">
          <q-avatar size="56px" class="q-mb-sm">
            <img src="https://cdn.quasar.dev/img/boy-avatar.png" />
          </q-avatar>
          <div class="text-weight-bold">{{ user.name }}</div>
          <div>{{ user.email }}</div>
        </div>
      </q-img>
    </q-drawer>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script>
import AuthService from "../services/auth.service";

const authService = new AuthService();

export default {
  name: "MainLayout",
  components: {},
  data() {
    return {
      drawer: false,
      isAnalyst: false,
      isManager: false,
      isUser: false
    };
  },
  created() {
    this.isAnalyst = this.user.userRole == 2 ? true : false;
    this.isManager = this.user.userRole == 1 ? true : false;
    this.isUser = this.user.userRole == 0 ? true : false;
  },
  methods: {
    logout() {
      authService.logout();
      this.$router.push("/auth/login");
    },
  },
  computed: {
    user() {
      return authService.getUserLogged();
    },
    linksData() {
      return [
        {
          title: "Início",
          icon: "home",
          link: "/home",
          canShow: this.isManager || this.isUser ,
        },
        {
          title: "Cadastrar usuário",
          icon: "admin_panel_settings",
          link: "/auth/register-manager",
          canShow: this.isAnalyst,
        },
        {
          title: "Usuários cadastrados",
          icon: "group",
          link: "/company/users-list",
          canShow: this.isAnalyst,
        },
        {
          title: "Empresas cadastradas",
          icon: "business",
          link: "/company/companies-list",
          canShow: this.isAnalyst,
        },
        {
          title: "Cadastrar produtos",
          icon: "shopping_basket",
          link: "/products/add-new",
          canShow: this.isManager,
        },
        {
          title: "Meus produtos",
          icon: "business",
          link: "/products/list",
          canShow: this.isManager,
        },
                {
          title: "Meus pagamentos",
          icon: "money",
          link: "/payments/list",
          canShow: this.isManager,
        },
        {
          title: "Cadastrar usuários",
          icon: "group_add",
          link: "/users/insert-user",
          canShow: this.isManager,
        },
        {
          title: "Meus usuários",
          icon: "group_add",
          link: "/users/list-user",
          canShow: this.isManager,
        },
        {
          title: "Minhas empresas",
          icon: "business",
          link: "/company/list-by-manager",
          canShow: this.isManager,
        },
      ];
    },
  },
};
</script>
