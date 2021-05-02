<template>
  <div>
    <q-table
      title="Empresas"
      :data="companiesArray"
      :columns="columns"
      row-key="name"
    >
      <template v-slot:body="props">
        <q-tr :props="props">
          <q-td key="corporateSocialName" :props="props">
            {{ props.row.corporateSocialName }}
          </q-td>
          <q-td key="fantasyName" :props="props">
            {{ props.row.fantasyName }}
          </q-td>
          <q-td key="cnpj" :props="props">
            {{ props.row.cnpj }}
          </q-td>
          <q-td key="status" :props="props">
            {{ status(props.row.status) }}
          </q-td>
           <q-td key="edit" :props="props">
            <q-btn size="sm" color="accent" round dense icon="create" @click="editCompany(props)"/>
          </q-td>
        </q-tr>
      </template>
    </q-table>
    <div class="row justify-center q-mt-md">
      <q-pagination
        v-model="pagination.page"
        color="grey-8"
        :max="pagination.rowsNumber"
        :max-pages="pagination.rowsPerPage"
        size="sm"
        @click="updateList"
      />
    </div>
    <edit-dialog-manager
      @updateList="updateList"
      @close="show = false"
      :company="companyProp"
      :show="show"
      v-if="show"
    />
  </div>
</template>

<script>
import editDialogManager from '../company/edit-dialog-company-manager'
export default {
  props: ["companiesArray"],
  name: "CompaniesListManagerComponent",
  components: {
    editDialogManager
  },
  data() {
    return {
      pagination: {
        sortBy: "desc",
        descending: false,
        page: 1,
        rowsPerPage: 4,
        rowsNumber: 0,
      },
      show: false,
      company: {},
      columns: [
        {
          name: "corporateSocialName",
          required: true,
          label: "Razão social",
          align: "left",
          field: (row) => row.corporateSocialName,
          format: (val) => `${val}`,
          sortable: true,
        },
        {
          name: "fantasyName",
          align: "left",
          label: "Nome fantasia",
          field: "Nome fantasia",
          field: (row) => row.fantasyName,
          format: (val) => `${val}`,
          sortable: true,
        },
        {
          name: "cnpj",
          align: "left",
          label: "CNPJ",
          field: "CNPJ",
          field: (row) => row.cnpj,
          format: (val) => `${val}`,
          sortable: true,
        },
        {
          name: "status",
          align: "left",
          label: "Status",
          field: "Status",
          field: (row) => row.status,
          format: (val) => `${val}`,
          sortable: true,
        },
        {
          name: "edit",
          align: "center",
          label: "Editar",
          field: "edit",
          field: (row) => row.userEnum,
          format: (val) => `${val}`,
          sortable: true,
        },
      ],
    };
  },
   methods: {
    updateList () {
      this.$emit('updateList', this.pagination.page, this.pagination.rowsPerPage)
    },
    editCompany(props) {
      this.company = props.row
      this.show = true
    },
    status(status) {
      switch (status) {
        case 0:
          return "ATIVO";
          break;
        case 1:
          return "INATIVO";
          break;
        case 2:
          return "BLOQUEADO";
          break;
      }
    },
  },
  computed: {
    companyProp () {
      let companyProp = {...this.company}
      return companyProp
    }
  },
  created() {
    this.pagination.rowsNumber = Math.ceil(this.total / this.pagination.rowsPerPage)
    console.log(JSON.stringify(this.pagination))
  },
};
</script>
