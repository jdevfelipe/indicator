<template>
  <div>
    <q-card class="my-card">
      <q-card-section :class="[bgCard, 'text-white', 'flex',  'justify-between']">
        <div class="flex column">
          <div class="text-h6">{{item.product.name}}</div>
          <div class="text-subtitle2">Status: {{status}}</div>
          <div class="text-subtitle2">Indicado: {{date}}</div>
        </div>
        <q-icon :name="icon" style="font-size: 2em;" />
      </q-card-section>

      <q-separator />

      <q-card-actions align="left">
        <q-btn no-caps flat color="primary" @click="show = true">Ver detalhes</q-btn>
      </q-card-actions>
    </q-card>
  <indication-detail :show="show" :text="item.obs" @close="show = false"/>
  </div>
</template>

<script>
import Util from '../../utils/util'
import IndicationDetail from '../commons/indication-detail.vue'

const util = new Util()
export default {
  name: 'myIndicationsCard',
  props: ['item'],
  components: {
    IndicationDetail
  },
  data () {
    return {
      show: false,
      text: ""
    }
  },
  computed : {
    bgCard () {
      switch (this.item.status) {
        case 0: return 'bg-primary'
          break;
        case 1: return 'bg-info'
          break;
        case 2: return 'bg-warning'
          break;
        case 3: return 'bg-secondary'
          break;
        case 4: return 'bg-positive'
          break;
        case 5: return 'bg-negative'
          break;
      }
    },
    status () {
      switch (this.item.status) {
        case 0: return 'POTENCIAL'
          break;
        case 1: return 'PIMEIRO CONTATO'
          break;
        case 2: return 'PROPOSTA FEITA'
          break;
        case 3: return 'NEGOCIAÇÃO'
          break;
        case 4: return 'SUCESSO'
          break;
        case 5: return 'FALHOU'
          break;
      }
    },
    icon () {
      switch (this.item.status) {
        case 0: return 'add_circle_outline'
          break;
        case 1: return 'headset_mic'
          break;
        case 2: return 'chat'
          break;
        case 3: return 'work'
          break;
        case 4: return 'check_circle_outline'
          break;
        case 5: return 'highlight_off'
          break;
      }
    },
    date () {
      return util.formatDate(this.item.indicationDate)
    }
  }
}
</script>
