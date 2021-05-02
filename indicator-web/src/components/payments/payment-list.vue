<template>
  <div>
    <q-table
      title="Pagamentos"
      :data="paymentsArray"
      :columns="columns"
      row-key="name"
    >
      <template v-slot:body="props">
        <q-tr :props="props">
          <q-td key="paymentDate" :props="props">
            {{ date(props.row.paymentDate) }}
          </q-td>
          <q-td key="indicator" :props="props">
            {{ props.row.indication.indicator.cpf }}
          </q-td>
          <q-td key="email" :props="props">
            {{ props.row.indication.indicator.email}}
          </q-td>
          <q-td key="indication" :props="props">
            {{ props.row.indication.document }}
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
  props: ["paymentsArray", "total"],
  name: "PaymentsListComponent",
  data() {
    return {
      pagination: {
        sortBy: 'desc',
        descending: false,
        page: 1,
        rowsPerPage: 2,
        rowsNumber: 0
      },
      columns: [
        {
          name: "paymentDate",
          required: true,
          label: "Data de pagamento",
          align: "left",
          field: (row) => row.paymentDate,
          format: (val) => `${val}`,
          sortable: true,
        },
        {
          name: "indicator",
          align: "left",
          label: "indicador",
          field: "indicator",
          field: (row) => row.indication.indicator.cpf,
          format: (val) => `${val}`,
          sortable: true,
        },
        {
          name: "email",
          align: "left",
          label: "email",
          field: "email",
          field: (row) => row.indication.indicator.email,
          format: (val) => `${val}`,
          sortable: true,
        },
        {
          name: "indication",
          align: "left",
          label: "indicação",
          field: "indicator",
          field: (row) => row.indication.document,
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
    date (paymentDate) {
      let date = paymentDate.split('T')[0];
      let day = date.split('-')[2]
      let month = date.split('-')[1]
      let year = date.split('-')[0]

      let finalDate = day + '/' + month + '/' + year

      return finalDate
    },
    status(status) {
      switch (status) {
        case 0:
          return "AGUARDANDO";
          break;
        case 1:
          return "CONFIRMADO";
          break;
        case 2:
          return "CONFIRMADO E NOTIFICADO";
          break;
        case 3:
          return "NÃO IDENTIFICADO";
          break;
      }
    }
  },
  created() {
    this.pagination.rowsNumber = Math.ceil(this.total / this.pagination.rowsPerPage)
    console.log(JSON.stringify(this.pagination))
  },
};
</script>
