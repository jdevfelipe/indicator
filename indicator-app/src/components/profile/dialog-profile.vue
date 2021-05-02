<template>
  <q-dialog
      v-model="isProfile"
      persistent
      :maximized="maximizedToggle"
      transition-show="slide-up"
      transition-hide="slide-down"
    >
    <q-card style="min-width: 300px">
        <q-bar class="bg-primary">
          <q-space />
          <q-btn dense flat icon="close" @click="$emit('closeProfile')">
            <q-tooltip content-class="bg-white text-primary">Close</q-tooltip>
          </q-btn>
        </q-bar>
      <div>
      <div class="row">
        <q-tabs
          v-model="tab"
          dense
          class="text-grey"
          active-color="primary"
          indicator-color="primary"
          align="justify"
          narrow-indicator
        >
          <q-tab name="profile" label="Perfil" />
          <q-tab name="bank" label="Banco" />
        </q-tabs>
        <q-separator />
      </div>
    </div>
    <q-tab-panels 
    v-model="tab"
    animated
    swipeable
    transition-prev="jump-right"
    transition-next="jump-left">
      <q-tab-panel name="profile" class="q-pa-none">
        <q-card-section>
          <div class="text-h6">Meus dados</div>
        </q-card-section>

        <q-card-section>
          <q-input outlined v-model="indicator.name" label="Nome" type="text" />
        </q-card-section>

        <q-card-section>
          <q-input outlined v-model="indicator.email" label="Email" type="text" />
        </q-card-section>

        <q-card-section>
          <q-input outlined v-model="indicator.cpf" label="CPF" disable/>
        </q-card-section>

        <q-card-actions align="right" class="q-mr-sm text-white">
          <q-btn class="bg-primary" label="Alterar" @click="updateProfile" no-caps/>
          <q-btn class="bg-primary" label="Fechar" @click="$emit('closeProfile')" no-caps/>
        </q-card-actions>
      </q-tab-panel>
      <q-tab-panel name="bank" class="q-pa-none">
        <q-card-section>
          <div class="text-h6">Banco</div>
        </q-card-section>
        <div v-for="(item, index) in indicator.paymentAccounts">
        <q-expansion-item
        expand-separator
        icon="money"
        :label="index + 1 + ' - Conta bancária'"
        :caption="item.bank"
      >

        <q-card-section>
          <q-select outlined v-model="item.bank" :options="options" label="Banco" />
        </q-card-section>

        <q-card-section>
          <div class="row">
              <q-input class="col-7" style="padding-bottom: 0" ref="agency" mask="####" outlined v-model="item.agency" label="Agência" type="text" :rules="[val => !!val || 'campo necessário']" />
          </div>
        </q-card-section>

        <q-card-section>
          <q-input outlined v-model="item.account" label="Conta" type="text"/>
        </q-card-section>
        <q-card-actions align="right" class="q-mr-sm text-white">
          <q-btn class="bg-primary" label="Alterar" @click="updateBank(item)" no-caps/>
          <q-btn class="bg-primary" label="Fechar" @click="$emit('closeProfile')" no-caps/>
        </q-card-actions>
      </q-expansion-item>
      </div>
      </q-tab-panel>
    </q-tab-panels>
    </q-card>
    </q-dialog>
</template>

<script>
import AuthService from '../../services/auth.service.js'
import NotifyService from '../../utils/notify.service.js'
import Util from '../../utils/util'

const notifyService = new NotifyService()
const authService = new AuthService();
const util = new Util();
export default {
  name: 'dialogProfile',
  props: ['isProfile', 'indicator'],
  data () {
    return {
      maximizedToggle: true,
      tab: 'profile',
      options: util.banksArr,
      digitVerif: ""
    }
  },
  methods: {
    updateBank (bank) {
      let vm = this
          this.$q.loading.show()
          if(this.digitVerif) {
            this.bank.Agency = this.bank.Agency + this.digitVerif
          }
    authService.updateBank(bank).then(res => {
      this.$q.loading.hide()
      notifyService.notify("Atualizado com sucesso!", "positive")
      authService.getIndicatorLoggedFromUpdate()
    }).catch(e => {
      this.$q.loading.hide()
        notifyService.notify(e.response.data.error, "negative");
    })
    },
    updateProfile () {
    this.$q.loading.show()
    authService.updateProfile(this.indicator).then(res => {
      this.$q.loading.hide()
      notifyService.notify("Atualizado com sucesso!", "positive")
      authService.getIndicatorLoggedFromUpdate()
    }).catch(e => {
        this.$q.loading.hide()
        notifyService.notify(e.response.data.error, "negative");
    })
    }
  },
}
</script>
