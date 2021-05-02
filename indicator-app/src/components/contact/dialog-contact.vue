<template>
    <q-dialog v-model="prompt" persistent>
      <q-card style="min-width: 300px">
        <q-card-section>
          <div class="text-h6">Fale com a gente!</div>
        </q-card-section>

        <q-card-section>
          <q-input dense v-model="contactForm.Email" label="Seu email" stack-label />
        </q-card-section>

        <q-card-section>
          <q-input dense v-model="contactForm.Name" label="Seu nome" stack-label />
        </q-card-section>

        <q-card-section>
          <q-input
            label="Sua mensagem"
            v-model="contactForm.message"
            dense
            autofocus
          />
        </q-card-section>

        <q-card-actions align="right" class="text-primary">
          <q-btn flat label="Cancelar" @click="$emit('closeContact')" no-caps />
          <q-btn flat label="Enviar" @click="sendMessage" no-caps/>
        </q-card-actions>
      </q-card>
    </q-dialog>
</template>

<script>
import ContactService from '../../services/contact.service.js'
import NotifyService from '../../utils/notify.service.js'

const contactService = new ContactService()
const notifyService = new NotifyService()
export default {
  name: 'dialog-contact',
  props: ['prompt', 'indicator'],
  data () {
    return {
    }
  },
  computed: {
    contactForm () {
      const obj = {
        Email: this.indicator.email,
        Name: this.indicator.name,
        message: ""
        }
        return obj;
      }
  },
  methods: {
    sendMessage() {
      this.$q.loading.show()
      contactService.sendMessage(this.contactForm).then(res => {
        this.$q.loading.hide()
        this.contactForm.message = ""
        this.$emit('closeContact')
        notifyService.notify("Enviado com sucesso", "positive")
      }).catch(e => {
        this.$q.loading.hide()
        notifyService.notify(e.response.data.error, "negative");
      })
    }
  },
}
</script>
