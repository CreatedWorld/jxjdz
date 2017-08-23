package avatar.module.exception;

import avatar.global.ClientCode;

/**
 * 房间用户状态不匹配
 */
public class RoomPlayerStateNotMatchException extends AbstractException {

    @Override
    public int getCode() {
        return ClientCode.USER_STATE_NOT_MATCH;
    }

    public RoomPlayerStateNotMatchException() {
        super();
    }

    public RoomPlayerStateNotMatchException(String message, Throwable cause) {
        super(message, cause);
    }

    public RoomPlayerStateNotMatchException(Throwable cause) {
        super(cause);
    }

    public RoomPlayerStateNotMatchException(String message) {
        super(message);
    }

}
