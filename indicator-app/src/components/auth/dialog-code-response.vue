<template>
  <q-dialog v-model="codeIsOk" persistent>
      <q-card style="min-width: 300px">
        <q-card-section>
          <div class="text-h6">{{title}}</div>
          <div class="text-subtitle2">{{message}}</div>
        </q-card-section>

        <q-card-actions align="right" class="text-primary">
          <q-btn flat label="Ok" v-if="type == 'NOT'" @click="($emit('closeDialogCodeResponse'))" />
          <q-btn flat label="Ok" v-if="type == 'OK'" @click="($emit('proceed'))" />
        </q-card-actions>
      </q-card>
    </q-dialog>
</template>

<script>
export default {
  name: 'dialogCodeResponse',
  props: ['codeIsOk', 'title', 'message', 'type'],
  data () {
    return {}
  }
}
</script>
