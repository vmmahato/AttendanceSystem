module.exports = {
    devServer: { 
        //https://github.com/EEParker/aspnetcore-vueclimiddleware/issues/9
        public: 'https://localhost:44362/' //Set SSL Port here for dev env
    },
  "transpileDependencies": [
    "vuetify"
  ]
}