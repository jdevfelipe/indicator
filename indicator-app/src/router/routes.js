const routes = [
  {
    path: '/', component: () => import('layouts/MainLayout.vue'),
    children: [
      { path: '/', redirect: '/validate' },
      { path: '/validate', component: () => import('pages/auth/validate-token.vue') },
      { path: '/home', component: () => import('pages/Index.vue') },
      { path: '/auth', component: () => import('pages/auth/Auth.vue') },
      { path: '/indications/home/:productId', component: () => import('pages/indications/Home.vue')},
      { path: '/indications/my-indications', component: () => import('pages/indications/my-indications.vue') },
      { path: '/products/home', component: () => import('pages/products/Home.vue') },
      { path: '/payments/my-payments', component: () => import('pages/payments/Home.vue') }
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
