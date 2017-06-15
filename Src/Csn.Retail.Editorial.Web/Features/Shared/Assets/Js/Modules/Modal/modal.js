/**
* Modal - reusable modal component
* @module
* @param {object} config - the initialisation config
*/

class Modal {

    constructor() {

        var $this = this;

        this._scope = document.querySelector('._c-modal');
        this._modalContent = this._scope.querySelector('._c-modal__content');
        this._closeModal = this._scope.querySelector('._c-modal__close');

        window.addEventListener('close-modal', function () { $this.closeModal(); });

        this._scope.addEventListener('click', function (event) {
            if (event.target.classList.contains('_c-modal') || event.target.className == '_c-modal__close') {
                $this.closeModal()
            }
        })
        this._closeModal.addEventListener('click', function () {
            $this.closeModal()
        })
        document.addEventListener('keyup', function (e) { $this.modalKeyPressed(e) })

    }

    closeModal() {
        this._scope.setAttribute('data-is-active', 'false');
        this._scope.className = '_c-modal';
        this._modalContent.innerHTML = ''; //Clear Modal

        let closeEvt = document.createEvent('Event')
        closeEvt.initEvent('modal.close', true, true)
        closeEvt.Response = this._scope;
        window.dispatchEvent(closeEvt);
    }

    modalKeyPressed(event) {
        if (this._scope.getAttribute('data-is-active') == true && event.keyCode === 27) {
            this.closeModal();
        }
    }

    updateView(html, className) {

        this._modalContent.innerHTML = html;
        className ? this._scope.classList.add(className) : ''; 
        this._scope.setAttribute('data-is-active', 'true');
        this._scope.classList.remove('loading'); 

        let evt = document.createEvent('Event')
        evt.initEvent('modal.content.added', true, true)
        evt.Response = this._scope;
        window.dispatchEvent(evt);

    }

    show(content, className, cb) {

        this._scope.classList.add('loading'); 

        if (className) {
            this.updateView(content, className)
        } else {
            this.updateView(content)
        }

        if (typeof cb === 'function') { cb() }
    }
}

export { Modal as default }