package avatar.module.exception;

/**
 * 业务过程中遇到的非正常情况，可以抛出此异常继承类，供业务入口api统一处理
 */
public abstract class AbstractException extends Exception {

    /**
     * 获取异常码
     */
    public abstract int getCode();

    public AbstractException() {
        super();
    }

    public AbstractException(String message) {
        super(message);
    }

    public AbstractException(String message, Throwable cause) {
        super(message, cause);
    }

    public AbstractException(Throwable cause) {
        super(cause);
    }

}
