<template>
  <q-btn-dropdown flat label="ALTERAR STATUS" :disable="this.indication.payments && this.indication.payments.length > 0">
    <q-list>
      <q-item clickable v-close-popup @click="potential">
        <q-item-section>
          <q-item-label>Potencial</q-item-label>
        </q-item-section>
      </q-item>

      <q-item clickable v-close-popup @click="firstContact">
        <q-item-section>
          <q-item-label>Primeiro contato</q-item-label>
        </q-item-section>
      </q-item>

      <q-item clickable v-close-popup @click="proposed">
        <q-item-section>
          <q-item-label>Proposta feita</q-item-label>
        </q-item-section>
      </q-item>

      <q-item clickable v-close-popup @click="negociation">
        <q-item-section>
          <q-item-label>Negociação</q-item-label>
        </q-item-section>
      </q-item>

      <q-item clickable v-close-popup @click="success">
        <q-item-section>
          <q-item-label>Negócio fechado</q-item-label>
        </q-item-section>
      </q-item>

      <q-item clickable v-close-popup @click="fail">
        <q-item-section>
          <q-item-label>Negócio recusado</q-item-label>
        </q-item-section>
      </q-item>
    </q-list>
  </q-btn-dropdown>
</template>

<script>
import IndicationsService from '../../services/indications.service'
import IndicationStatus from '../../enums/indication-status.enum'
import NotifyService from '../../utils/notify.service'

const indicationsService = new IndicationsService();
const notifyService = new NotifyService();
const indicationStatus = IndicationStatus;
export default {
  name: "ChangeStepBtnComponent",
  props: ["indication"],
  data() {
    return {};
  },
  created () {
  },
  methods: {
    potential() {
      this.$q.loading.show();
      indicationsService.changeStatus(this.indication.id, indicationStatus.IndicationStatus.POTENTIAL).then(res => {
        this.$emit('updateStatus')
        this.$q.loading.hide();
        notifyService.notify("Alterado com sucesso!", 'positive')
      }).catch(e => {
        this.$q.loading.hide();
        notifyService.notify(e.response.data.error, 'negative')
      })
    },
    firstContact() {
      this.$q.loading.show();
      indicationsService.changeStatus(this.indication.id, indicationStatus.IndicationStatus.FIRST_CONTACT).then(res => {
        this.$emit('updateStatus')
        this.$q.loading.hide();
        notifyService.notify("Alterado com sucesso!", 'positive')
      }).catch(e => {
        this.$q.loading.hide();
        notifyService.notify(e.response.data.error, 'negative')
      })
    },
    proposed() {
      this.$q.loading.show();
      indicationsService.changeStatus(this.indication.id, indicationStatus.IndicationStatus.PROPOSED).then(res => {
        this.$emit('updateStatus')
        this.$q.loading.hide();
        notifyService.notify("Alterado com sucesso!", 'positive')
      }).catch(e => {
        this.$q.loading.hide();
        notifyService.notify(e.response.data.error, 'negative')
      })
    },
    negociation() {
      this.$q.loading.show();
      indicationsService.changeStatus(this.indication.id, indicationStatus.IndicationStatus.NEGOCIATION).then(res => {
        this.$emit('updateStatus')
        this.$q.loading.hide();
        notifyService.notify("Alterado com sucesso!", 'positive')
      }).catch(e => {
        this.$q.loading.hide();
        notifyService.notify(e.response.data.error, 'negative')
      })
    },
    success() {
      this.$q.loading.show();
      indicationsService.changeStatus(this.indication.id, indicationStatus.IndicationStatus.SUCCESS).then(res => {
        this.$emit('updateStatus')
        this.$q.loading.hide();
        notifyService.notify("Alterado com sucesso!", 'positive')
      }).catch(e => {
        this.$q.loading.hide();
        notifyService.notify(e.response.data.error, 'negative')
      })
    },
    fail() {
      this.$q.loading.show();
      indicationsService.changeStatus(this.indication.id, indicationStatus.IndicationStatus.FAILED).then(res => {
        this.$emit('updateStatus')
        this.$q.loading.hide();
        notifyService.notify("Alterado com sucesso!", 'positive')
      }).catch(e => {
        this.$q.loading.hide();
        notifyService.notify(e.response.data.error, 'negative')
      })
    },
  },
};
</script>
