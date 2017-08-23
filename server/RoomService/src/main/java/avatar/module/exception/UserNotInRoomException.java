package avatar.module.exception;

import avatar.global.ClientCode;

/**
 * 用户不在房间中
 */
public class UserNotInRoomException extends AbstractException {

    @Override
    public int getCode() {
        return ClientCode.NOT_IN_ROOM;
    }

    public UserNotInRoomException(String message, Throwable cause) {
        super(message, cause);
    }

    public UserNotInRoomException(Throwable cause) {
        super(cause);
    }

    public UserNotInRoomException() {
        super();
    }

    public UserNotInRoomException(String message) {
        super(message);
    }
}
