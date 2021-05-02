<template>
  <q-page class="flex column justify-start items-center q-pl-md q-pr-md q-pt-lg">
    <div v-for="card in cards" style="width: 100%;" class="q-mb-lg">
      <home-card :item="card" :indicator="indicator"></home-card>
    </div>
  </q-page>
</template>

<script>
import AuthService from '../services/auth.service'
import homeCard from '../components/dashboard/home-card'
const authService = new AuthService()
export default {
  name: 'PageIndex',
  components: {
    homeCard,
    isLogged: false
  },
  data () {
    return {
      cards: [
        {
          id: 1,
          name: "Indicar",
          color: "bg-primary",
          textColor: "text-white",
          link: "/indication",
          icon: "people_alt"
        },
        {
          id: 2,
          name: "Indicações",
          color: "bg-secondary",
          textColor: "text-white",
          link: "/indications/my-indications",
          icon: "account_balance_wallet"
        },
        {
          id: 3,
          name: "Meus dados",
          color: "bg-info",
          textColor: "text-white",
          link: "/indication",
          icon: "account_box"
        },
        {
          id: 4,
          name: "Contato",
          color: "bg-warning",
          textColor: "text-white",
          link: "/indication",
          icon: "contact_support"
        },
      ]
    }
  },
  computed: {
    indicator () {
      return authService.getIndicatorLogged()
    }
  }
}
</script>
