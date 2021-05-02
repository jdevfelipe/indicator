<template>
  <div>
    <q-card class="my-card">
      <q-card-section class="flex justify-between bg-primary text-white">
        <div class="text-h6">{{item.name}}</div>
        <div class="text-subtitle2">{{item.company.fantasyName}}</div>
        <q-icon name="work_outline" style="font-size: 2em;" />
      </q-card-section>

      <q-separator />

      <q-card-actions class="flex justify-between">
        <q-btn no-caps flat color="primary">Saber mais</q-btn>
        <q-btn outline color="primary" label="Indicar" @click="goToIndication"/>
      </q-card-actions>
    </q-card>
  </div>
</template>

<script>
export default {
  name: 'productHomeCard',
  props: ['item'],
  data () {
    return {}
  },
  created () {
    console.log(this.item)
  },
  methods: {
    goToIndication () {
      console.log(this.item.id)
      this.$router.push(`/indications/home/${this.item.id}`)
    }
  }
}
</script>
