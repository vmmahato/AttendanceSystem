<template>
    <v-card class="customFontSize">
        <v-card-title class="headline" color="red darken-1">Delete?</v-card-title>
        <v-card-text class="text-left subtitle-1">Do you want to delete
            <span class="font-weight-bold red--text" v-text="itemTitle + ''+'?'"/>
            </v-card-text>
        <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="teal darken-1" text @click="closeForm()">No</v-btn>
            <v-btn color="red darken-1" text @click="saveData()">Yes</v-btn>
        </v-card-actions>
    </v-card>
</template>

<script>
    import axios from 'axios'

    export default {
        name: "TheDeleteForm",
        props: {
            deleteUrl: {
                type: String,
                required: true,
            },
            title:{
                type: String,
                required:true
            }
        },
        computed: {
            url() {
                return this.deleteUrl;
            },
            itemTitle(){
              return this.title
            }
        },
        data() {
            return {
                snackbar: {
                    data: {},
                    message: "",
                    color: ""
                }
            }
        },
        methods: {
            closeForm() {
                this.$emit('closeDialog',this.snackbar)
            },
            async saveData() {
                try {
                    const {data} = await axios.post(this.url, {})
                    this.snackbar.data = data
                    this.snackbar.message = data.success ? 'Data Deleted Successfully' : data.errors
                    this.snackbar.color=data.success?"success":'error'
                    this.closeForm()
                } catch (e) {
                    console.log(e)
                }
            }
        },
    }
</script>

<style scoped>

</style>