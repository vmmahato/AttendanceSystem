<template>
    <v-layout>
        <v-text-field
                :value="nepaliDate[0]"
                :label="labelFrom"
                :error-messages="errorMessages"
                required
                readonly
                ref="vtext"
        ></v-text-field>
        <v-text-field
                :value="nepaliDate[1]"
                :label="labelTo"
                required
                readonly
                ref="vtext2"
        ></v-text-field>
    </v-layout>
</template>

<script>
    export default {
        name: "nepaliPicker",
        props: {
            date: {
                default: '',
                type: String,
            },
            labelFrom:{
                type:String,
                default:''
            },
            labelTo:{
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
            }
        },
        data() {
            return {
                nepaliDate: [],
                englishDate:[],
                vl:'vtext2'
            }
        },
        methods: {
        },
        mounted: function () {
            let self = this
            this.$refs.vtext.$el.firstChild.firstChild.lastChild.lastChild.nepaliDatePicker({
                onChange: function(e){
                    console.log(e)
                    self.nepaliDate[0] = e.bs
                    self.englishDate[0] = e.ad
                    self.$emit('input',self.englishDate)
                },
                ndpYear:true,
                ndpMonth:true
            })
            this.$refs[this.vl].$el.firstChild.firstChild.lastChild.lastChild.nepaliDatePicker({
                onChange: function(f){
                    console.log(f)
                    self.nepaliDate[1] = f.bs
                    self.englishDate[1] = f.ad
                    self.$emit('input',self.englishDate)
                },
                ndpYear:true,
                ndpMonth:true
            })
        }
    }
</script>

<style scoped>

</style>