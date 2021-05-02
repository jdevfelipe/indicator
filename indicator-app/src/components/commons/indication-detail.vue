<template>
  <q-dialog v-model="show" full-width>
      <q-card>
        <q-card-section>
          <div class="text-h6">Detalhes</div>
        </q-card-section>

        <q-card-section class="q-pt-none">
          <p>{{text}}</p>
        </q-card-section>

        <q-card-actions align="right">
          <q-btn flat label="OK" color="primary" @click="$emit('close')" />
        </q-card-actions>
      </q-card>
    </q-dialog>
</template>

<script>
export default {
  name: 'PaymentDialog',
  props: ['text', 'show'],
  data () {
    return {}
  }
}
</script>
