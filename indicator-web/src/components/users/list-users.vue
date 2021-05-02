<template>
  <div>
    <q-table
      title="Usuários"
      :data="usersArray"
      :columns="columns"
      row-key="name"
    >
      <template v-slot:body="props">
        <q-tr :props="props">
          <q-td key="name" :props="props">
            {{ props.row.name }}
          </q-td>
          <q-td key="cpf" :props="props">
            {{ props.row.cpf }}
          </q-td>
          <q-td key="email" :props="props">
            {{ props.row.email }}
          </q-td>
          <q-td key="userRole" :props="props">
            {{ roles(props.row.userRole) }}
          </q-td>
          <q-td key="userEnum" :props="props">
            {{ status(props.row.userEnum) }}
          </q-td>
          <q-td key="edit" :props="props">
            <q-btn size="sm" color="accent" round dense icon="create" @click="editUser(props)"/>
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
    <edit-dialog-user-company @updateList="updateList" @close="show = false" :user="userProp" :show="show" v-if="show"/>
  </div>
</template>

<script>
import editDialogUserCompany from '../users/edit-dialog-user-company'
export default {
  props: ["usersArray", "total"],
  name: "ListUsersComponent",
  components : {
    editDialogUserCompany
  },
  data() {
    return {
       pagination: {
        sortBy: 'desc',
        descending: false,
        page: 1,
        rowsPerPage: 2,
        rowsNumber:0
      },
      show: false,
      user: {},
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
          name: "cpf",
          align: "left",
          label: "Documento",
          field: "Documento",
          field: (row) => row.cpf,
          format: (val) => `${val}`,
          sortable: true,
        },
        {
          name: "email",
          align: "left",
          label: "Email",
          field: "Email",
          field: (row) => row.email,
          format: (val) => `${val}`,
          sortable: true,
        },
        {
          name: "userRole",
          align: "left",
          label: "Cargo",
          field: "Cargo",
          field: (row) => row.userRole,
          format: (val) => `${val}`,
          sortable: true,
        },
        {
          name: "userEnum",
          align: "left",
          label: "Status",
          field: "Status",
          field: (row) => row.userEnum,
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
      console.log(JSON.stringify(this.pagination))
      this.$emit('updateList', this.pagination.page, this.pagination.rowsPerPage)
    },
    editUser(props) {
      console.log("here")
      this.user = props.row
      this.show = true
    },
    roles(role) {
      switch (role) {
        case 0:
          return "USUÁRIO";
          break;
        case 1:
          return "GERENTE DE CONTA";
          break;
        case 2:
          return "ANALISTA";
          break;
      }
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
    userProp () {
      let userProp = {...this.user}
      return userProp
    }
  },
  created() {
    this.pagination.rowsNumber = Math.ceil(this.total / this.pagination.rowsPerPage)
    console.log(JSON.stringify(this.pagination))
  },
};
</script>