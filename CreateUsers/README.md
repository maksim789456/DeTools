# CreateUsers

Программа для одновременного и автоматического создания пользователей для студентов сдающих "Демонстрационный экзамен"

## Использование

### Скачайте программу

[Тут](https://github.com/maksim789456/DeTools/releases) или соберите самостоятельно из исходников

### Настройте окружение

<details>
<summary>Gogs</summary>

Необходима учетная запись администратора.
Создайте новый API ключ в настройках пользователя (http://<адрес сервера>/user/settings/applications).
Добавьте адрес сервера в переменную окружения ``GOGS_URL`` и API ключ в ``GOGS_ADMIN_TOKEN``.
Так же можно настроить email адрес с которым будут создаваться пользователи через переменную ``GIT_EMAIL``. По
умолчанию стоит ``de.test.ru``
</details>

<details>
<summary>Gitea</summary>

Необходима учетная запись администратора.
Создайте новый API ключ в настройках пользователя (http://<адрес сервера>/user/settings/applications). 
При создании выдайте для ключа права ``admin`` на чтение и запись.
Добавьте адрес сервера в переменную окружения ``GITEA_URL`` и API ключ в ``GITEA_ADMIN_TOKEN``.
Так же можно настроить email адрес с которым будут создаваться пользователи через переменную ``GIT_EMAIL``. По
умолчанию стоит ``de.test.ru``
</details>

<details>
<summary>MS SqlServer</summary>

Так же необходима учетная запись администратора (или аналогичная с правами на создание пользователей и баз данных).
Строку для подключения в формате [connection string](https://www.connectionstrings.com/sql-server/) добавьте в
переменную ``SQLSERVER_CONNECTION_STRING``
</details>

<details>
<summary>MySql</summary>

Аналогично, необходима учетная запись администратора (или аналогичная с правами на создание пользователей и баз данных).
Строку для подключения в формате [connection string](https://www.connectionstrings.com/mysql/) добавьте в переменную
``MYSQL_CONNECTION_STRING``
</details>

Так же поддерживается файлы ``.env`` формата. Пример: ([файлом](https://github.com/maksim789456/DeTools/blob/master/CreateUsers/.env.example))

```ini
GOGS_URL = http://localhost:3000/
GOGS_ADMIN_TOKEN = MY_COOL_API_KEY
GOGS_EMAIL = de.test.ru
SQLSERVER_CONNECTION_STRING = "Server=localhost;Database=master;User Id=sa;Password=MY_COOL_PASS;TrustServerCertificate=True"
MYSQL_CONNECTION_STRING = "Server=localhost;Uid=root;Pwd=MY_COOL_PASS"
```
При каждом старте программа сама проверит наличие всех переменных окружения, доступность серверов и прав доступа.