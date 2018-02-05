var app = Sys.Application;
function ApplicationInit(sender) {
    var prm = Sys.WebForms.PageRequestManager.getInstance();

    if (!prm.get_isInAsyncPostBack()) {
        prm.add_pageLoaded(PageLoaded);
    }
}
function pageLoad() {


    KE.init({
        id: 'txtContent',
        imageUploadJson : '../../../handler/upload_json.ashx',
        fileManagerJson : '../../../handler/file_manager_json.ashx',
        allowFileManager: true,
        afterCreate: function(id) {
            KE.util.focus(id);
        }
    });
    timename = setTimeout("KE.create('txtContent');", 50); 
}

function PageLoaded(sender, args) {
    KE.create('txtContent');
}