<template>
  <q-page class="flex column justify-start items-center q-pl-md q-pr-md q-pt-lg">
    <div style="width: 100%;" class="q-mb-lg">
      <searchBar ref="searchBar" @getProductsBySearch="getIndicationssBySearch"/>
    </div>
      <div style="width: 100%;" class="q-mb-lg" v-for="indication in IndiArr">
        <MyIndicationsCard :item="indication" />
      </div
          <div class="q-pa-lg flex flex-center">
      <q-pagination
          v-model="current"
          color="primary"
          :max="maxPages"
          :max-pages="3"
          :ellipses="false"
          :boundary-numbers="false"
          @click="$refs.searchBar.text ? getIndicationssBySearch($refs.searchBar.text) : getIndications('fromCreated', 'CREATED' , current, 3)"
        >
        </q-pagination>
  </div>
  </q-page>
</template>

<script>
import MyIndicationsCard from '../../components/indications/my-indications-card'
import searchBar from '../../components/commons/search-bar'
import IndicationService from '../../services/indication.service'
import NotifyService from '../../utils/notify.service'

const indicationService = new IndicationService();
const notifyService = new NotifyService();
export default {
  name: 'Indications',
  components: {
    MyIndicationsCard,
    searchBar
  },
  data() {
    return {
      indications: [],
      intervalIndication: '',
      current: 1,
      maxPages: 0
    }
  },
  methods: {
    getIndications(from, text, page, limit) {
      if (from === 'fromCreated') {
        this.$q.loading.show()
      }
      indicationService.getIndications(text, page, limit).then(res => {
        if (from === 'fromCreated') {
          this.$q.loading.hide()
        }
        this.indications = res.data.indications
        this.maxPages = Math.ceil(res.data.total /3)
      }).catch(e => {
        this.indications = []
        this.$q.loading.hide()
      })
    },
    getIndicationssBySearch(text) {
      this.$q.loading.show()
      this.current = 1
      indicationService.getIndications(text, this.current, 3).then(res => {
        this.indications = res.data.indications
        this.maxPages = Math.ceil(res.data.total /3)
        this.$q.loading.hide()
      }).catch(e => {
        this.indications = []
        notifyService.notify(e.response.data.error, 'info')
        this.$q.loading.hide()
      })

    },
    // pollCardStatus () {
    //   debugger
    //   let vm = this
    //   let term = vm.$refs.searchBar.text ? vm.$refs.searchBar.text : "POLL"
    //   this.intervalIndication = setInterval(function(){
    //       term ? vm.getIndicationssBySearch(term) : vm.getIndications('fromCreated', 'POLL' , current, maxPages)
    //      }, 10000);
    // }
  },
  mounted () {
    this.getIndications('fromCreated', "CREATED", 1, 3)
    //this.pollCardStatus()
  },
  beforeDestroy () {
    clearInterval(this.interval);
  },
  computed: {
    IndiArr () {
      return this.indications
    }
  }
}
</script>
