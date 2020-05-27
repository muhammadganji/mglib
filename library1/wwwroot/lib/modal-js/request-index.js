(function ($) {
    function ManageRequestBook() {// ManageRequestBook اسم این تابع باید با کلاس کنترلی مورد استفاده یکسان باشد
        var $this = this;

        function initilizeModel() {
            $("#modal-action-request").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new ManageRequestBook(); // Author اسم این تابع باید با کلاس کنترلی مورد استفاده یکسان باشد
        self.init();
    })
}(jQuery))
