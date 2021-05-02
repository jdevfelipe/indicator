<template>
  <q-input outlined bottom-slots v-model="text" label="Buscar" dense>
    <template v-slot:append>
      <q-icon v-if="text !== ''" name="close" @click="text = ''" class="cursor-pointer" />
      <q-icon name="search" @click="$emit('getProductsBySearch', text)"/>
    </template>
  </q-input>
</template>

<script>
export default {
  name: 'searchBar',
  data () {
    return {
      text: ''
    }
  }
}
</script>
