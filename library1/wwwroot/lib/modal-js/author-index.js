(function ($) {
    function Author() {// Author اسم این تابع باید با کلاس کنترلی مورد استفاده یکسان باشد
        var $this = this;

        function initilizeModel() {
            $("#modal-action-author").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new Author(); // Author اسم این تابع باید با کلاس کنترلی مورد استفاده یکسان باشد
        self.init();
    })
}(jQuery))
