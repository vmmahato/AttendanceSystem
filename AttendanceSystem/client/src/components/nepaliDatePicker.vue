<template>
    <div>
        <v-text-field class="d-none">
        </v-text-field>
        <v-text-field
                :value="nepaliDate"
                :label="label"
                :error-messages="errorMessages"
                required
                readonly
                ref="vtext"
                @blur="sendDate"
                :outlined="outlined"
                :disabled="disabled"
        ></v-text-field>
    </div>
</template>

<script>
    export default {
        name: "nepaliPicker",
        props: {
            date: {
                default: '',
                type: String,
            },
            label:{
                type:String,
                default:''
            },
            max:{
                type:String,
                required:false
            },
            min:{
                type:String,
                required:false
            },
            errorMessages:{
                required:false
            },
            outlined:{
                type:Boolean,
                required:false
            },
            disabled:{
                type:Boolean,
                required:false
            }
        },
        data() {
            return {
                nepaliDate: null,
                englishDate:this.$attrs.value || null
            }
        },
        computed:{
            nepaliMax(){
                if(this.max){
                    let englishDateObject = window.NepaliFunctions.ConvertToDateObject(this.max, "YYYY-MM-DD")
                    let nepaliDateObject = window.NepaliFunctions.AD2BS(englishDateObject, "YYYY-MM-DD")
                    return window.NepaliFunctions.ConvertDateFormat(nepaliDateObject, "YYYY-MM-DD")
                }
                return null
            },
            nepaliMin(){
               if(this.min){
                   let englishDateObject = window.NepaliFunctions.ConvertToDateObject(this.min, "YYYY-MM-DD")
                   let nepaliDateObject = window.NepaliFunctions.AD2BS(englishDateObject, "YYYY-MM-DD")
                   return window.NepaliFunctions.ConvertDateFormat(nepaliDateObject, "YYYY-MM-DD")
               }
               return null
            },
            editDate(){
                if(this.$attrs.value){
                    let englishDateObject = window.NepaliFunctions.ConvertToDateObject(this.$attrs.value, "YYYY-MM-DD")
                    let nepaliDateObject = window.NepaliFunctions.AD2BS(englishDateObject, "YYYY-MM-DD")
                    return window.NepaliFunctions.ConvertDateFormat(nepaliDateObject, "YYYY-MM-DD")
                }
               return null
            }
        },
        methods: {
            init () {
                console.log('i am inside init and the attrs is', this.$attrs.value)
                let self = this
                console.log('self check : ', this)
                console.log('self check : ', this.$refs.vtext.$el.firstChild.firstChild.lastChild.lastChild)
                this.$refs.vtext.$el.firstChild.firstChild.lastChild.lastChild.nepaliDatePicker({
                    onChange: function(e){
                        console.log(e)
                        self.nepaliDate = e.bs
                        self.englishDate = e.ad
                        self.$emit('input',self.englishDate)
                    },
                    disableAfter:self.nepaliMax,
                    disableBefore:self.nepaliMin,
                    ndpYear:true,
                    ndpMonth:true
                })
            },
            setDate(){
                if(this.editDate){
                    console.log('this.editDate : ', this.editDate)
                    this.nepaliDate = this.editDate
                    this.englishDate = this.$attrs.value
                    console.log('this.editDate : ', this.nepaliDate)
                    console.log('this.editDate : ', this.$attrs.value)
                }
            },
            sendDate(){
                this.$emit('input',this.englishDate||null)
            }
        },
        mounted() {
            this.setDate()
            this.init()
        },
        created() {

        },
        watch:{
            min:{
                handler(){
                    if(this.min){
                        this.init()
                    }
                }
            },
            max:{
                handler(){
                    if(this.max){
                        this.init()
                    }
                }
            }
        }
    }
</script>

<style scoped>

</style>