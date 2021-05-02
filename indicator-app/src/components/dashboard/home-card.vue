<template>
  <div>
    <q-card class="my-card" @click="open(item.id)">
      <q-card-section :class="[item.color, item.textColor, 'flex', 'justify-between']">
        <div class="text-h6">{{item.name}}</div>
        <q-icon :name="item.icon" style="font-size: 2em;" />
      </q-card-section>

      <q-separator />
      <q-card-actions align="left">
        <q-btn no-caps flat color="primary" v-if="item.id == 4" @click="isContact = true">Preciso de ajuda!</q-btn>
        <q-btn no-caps flat color="primary" v-if="item.id == 3" @click="isProfile = true">Abrir</q-btn>
        <q-btn no-caps flat color="primary" v-if="item.id == 2" @click="$router.push('/indications/my-indications')">Abrir</q-btn>
        <q-btn no-caps flat color="primary" v-if="item.id == 1" @click="$router.push('/products/home')">Abrir</q-btn>
      </q-card-actions>
    </q-card>
    <dialog-contact @closeContact="closeContact" :prompt="isContact" :indicator="indicator" />
    <dialog-profile :isProfile="isProfile" :indicator="indicator"  @closeProfile="closeProfile" />
  </div>
</template>

<script>
import dialogContact from '../contact/dialog-contact'
import dialogProfile from '../profile/dialog-profile'

export default {
  name: 'home-card',
  props: ['item', 'indicator'],
  components: {
    dialogContact,
    dialogProfile
  },
  data () {
    return {
    isContact: false,
    isProfile: false
    }
  },
  methods: {
    open(id) {
      switch (id) {
        case 4:
          this.isContact = true
          break;
        case 3:
          this.isProfile = true
          break;
        case 2:
          this.$router.push('/indications/my-indications')
          break;
        case 1:
          this.$router.push('/products/home')
          break;
        default:
          break;
      }
    },
    closeContact () {
      this.isContact = false
    },
    closeProfile () {
      this.isProfile = false
    }
  }
}
</script>
<style scoped>
</style>
