<template>
  <div>
    <q-table
      title="Produtos"
      :data="productsArray"
      :columns="columns"
      row-key="name"
    >
      <template v-slot:body="props">
        <q-tr :props="props">
          <q-td key="name" :props="props">
            {{ props.row.name }}
          </q-td>
          <q-td key="indicationPrice" :props="props">
            {{ props.row.indicationPrice.toLocaleString('pt-br',{style: 'currency', currency: 'BRL'}) }}
          </q-td>
          <q-td key="company" :props="props">
            {{ props.row.company.fantasyName }}
          </q-td>
          <q-td key="company" :props="props">
            {{ props.row.indications.length }}
          </q-td>
          <q-td key="status" :props="props">
            {{ status(props.row.status) }}
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
  </div>
</template>

<script>
export default {
  props: ["productsArray", "total"],
  name: "ProductsListComponent",
  data() {
    return {
      pagination: {
        sortBy: 'desc',
        descending: false,
        page: 1,
        rowsPerPage: 1,
        rowsNumber: 0
      },
      columns: [
        {
          name: "name",
          required: true,
          label: "Nome",
          align: "left",
          field: (row) => row.name,
          format: (val) => `${val}`,
          sortable: true,
        },
        {
          name: "indicationPrice",
          align: "left",
          label: "Preço por indicação",
          field: "indicationPrice",
          field: (row) => row.indicationPrice,
          format: (val) => `${val}`,
          sortable: true,
        },
        {
          name: "company",
          align: "left",
          label: "Empresa",
          field: "company",
          field: (row) => row.company,
          format: (val) => `${val}`,
          sortable: true,
        },
        {
          name: "indications",
          align: "left",
          label: "N° de indicações",
          field: "indications",
          field: (row) => row.indications,
          format: (val) => `${val}`,
          sortable: true,
        },
        {
          name: "status",
          align: "left",
          label: "Status",
          field: "status",
          field: (row) => row.status,
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
  created() {
    this.pagination.rowsNumber = Math.ceil(this.total / this.pagination.rowsPerPage)
    console.log(JSON.stringify(this.pagination))
  },
};
</script>
