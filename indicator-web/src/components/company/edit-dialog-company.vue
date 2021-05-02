<template>
<q-dialog v-model="show" persistent>
      <q-card style="min-width: 350px">
        <q-card-section>
          <div class="text-h6">Dados</div>
        </q-card-section>

        <q-card-section class="q-pt-none">
          <q-input dense v-model="company.fantasyName" autofocus @keyup.enter="prompt = false" />
        </q-card-section>
                <q-card-section class="q-pt-none">
          <q-input dense v-model="company.corporateSocialName" autofocus @keyup.enter="prompt = false" />
        </q-card-section>
        <q-card-section class="q-pt-none">
          <q-input dense v-model="company.cnpj" disable autofocus @keyup.enter="prompt = false" />
        </q-card-section>
        <q-card-section class="q-pt-none">
          <q-input dense v-model="company.cpf" autofocus @keyup.enter="prompt = false" />
        </q-card-section>
        <q-card-section class="q-pt-none">
          <q-select outlined v-model="role" :options="optionsRole" map-options emit-value label="Standard" />
        </q-card-section>

        <q-card-actions align="right" class="text-primary">
          <q-btn flat label="Cancelar" @click="$emit('close')" />
          <q-btn flat label="SALVAR" @click="edit()" />
        </q-card-actions>
      </q-card>
    </q-dialog>
</template>

<script>
import CompaniesService from '../../services/companies.service'
import NotifyService from '../../utils/notify.service'

const companiesService = new CompaniesService();
const notifyService = new NotifyService();
export default {
  name: 'editDialogCompany',
  props: ['show', 'company'],
  data () {
    return {
      optionsRole: [
        {
          label: "BLOQUEADO",
          value: 2
        },
        {
          label: "INATIVO",
          value: 1
        },
        {
          label: "ATIVO",
          value: 0
        },
      ],
      role: ""
    }
  },
  mounted() {
    let vm = this
    vm.role = vm.company.status
  },
  methods: {
    edit () {
      this.company.status = this.role
      this.$q.loading.show();
      companiesService.updateCompany(this.company).then(res => {
        this.$q.loading.hide();
        this.$emit('updateList')
        notifyService.notify("Alterado com sucesso!", 'positive')
        this.$emit('close')
      }).catch(e => {
        notifyService.notify("Algo deu errado", 'warning')
      })
    }
  }
}
</script>
