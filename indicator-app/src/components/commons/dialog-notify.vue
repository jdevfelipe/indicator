<template>
  <q-dialog v-model="model" persistent transition-show="scale" transition-hide="scale">
      <q-card :class="[color, 'text-white']" style="width: 300px">
        <q-card-section class="flex justify-between">
          <div class="text-h6">{{title}}</div>
          <q-icon :name="icon" size="2em" />
        </q-card-section>

        <q-card-section class="q-pt-none">
          {{message}}
        </q-card-section>

        <q-card-actions align="right" class="bg-white text-teal">
          <q-btn flat label="OK" @click="$emit('closeNotify')" />
        </q-card-actions>
      </q-card>
  </q-dialog>
</template>

<script>
export default {
  name: 'DialogNotify',
  props: ['model', 'title', 'message', 'icon', 'color'],
  data () {
    return {}
  }
}
</script>
