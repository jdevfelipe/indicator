
const routes = [
  {
    path: '/',
    component: () => import('layouts/MainLayout.vue'),
    children: [
      { path: '/', redirect: '/validate' },
      { path: '/home', component: () => import('pages/Index.vue')},
      { path: '/validate', component: () => import('pages/auth/validate-token.vue') },
      { path: '/auth/login', component: () => import('pages/auth/login.vue') },
      { path: '/auth/register-manager', component: () => import('pages/auth/registerManager.vue') },
      { path: '/company/users-list', component: () => import('pages/company/users-list.vue') },
      { path: '/company/companies-list', component: () => import('pages/company/companies-list.vue') },
      { path: '/products/add-new', component: () => import('pages/products/insert-product.vue') },
      { path: '/products/list', component: () => import('pages/products/list-products.vue') },
      { path: '/users/insert-user', component: () => import('pages/users/insert-user.vue') },
      { path: '/users/list-user', component: () => import('pages/users/list-users.vue') },
      { path: '/payments/list', component: () => import('pages/payments/payment-list.vue') },
      { path: '/company/list-by-manager', component: () => import('pages/company/companies-list-manager.vue') }
    ]
  },

  // Always leave this as last one,
  // but you can also remove it
  {
    path: '*',
    component: () => import('pages/Error404.vue')
  }
]

export default routes
