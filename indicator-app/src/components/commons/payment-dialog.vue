<template>
  <q-dialog v-model="show" full-width>
      <q-card>
        <q-card-section>
          <div class="text-h6">Comprovante</div>
        </q-card-section>

        <q-card-section class="q-pt-none">
          <q-img
          :src="img"
          style="width: 100%"
        />
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
  props: ['img', 'show'],
  data () {
    return {}
  }
}
</script>
